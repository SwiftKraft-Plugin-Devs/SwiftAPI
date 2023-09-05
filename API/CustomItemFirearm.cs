using CustomPlayerEffects;
using Hints;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.BasicMessages;
using InventorySystem.Items.Firearms.Modules;
using PlayerRoles;
using PlayerStatsSystem;
using PluginAPI.Core;

namespace CustomItemAPI.API
{
    /// <summary>
    /// This class is a base for custom firearms/weapons.
    /// </summary>
    public class CustomItemFirearm : CustomItemEquippable
    {
        public CustomFirearmData Data;

        bool delay;

        public override void Equip(Player _player, ushort _itemSerial)
        {
            if (_player.CurrentItem is Firearm f)
                ResetFirearm(f, this);

            base.Equip(_player, _itemSerial);
        }

        public override bool Drop(Player _player, ItemBase _item)
        {
            if (_item is Firearm f)
                ResetFirearm(f, this);

            return base.Drop(_player, _item);
        }

        /// <summary>
        /// Called when the custom weapon is shot.
        /// </summary>
        /// <param name="_player"></param>
        /// <param name="_gun"></param>
        public virtual void Shoot(Player _player, Firearm _gun)
        {
            if (delay)
                return;

            ResetFirearm(_gun, this);

            if (Data.ExtraBulletCount > 0)
            {
                delay = true;

                for (int i = 0; i < Data.ExtraBulletCount; i++)
                {
                    ResetFirearm(_gun, this);

                    _gun.HitregModule.ClientCalculateHit(out ShotMessage msg);
                    _gun.HitregModule.ServerProcessShot(msg);
                }

                delay = false;
            }
        }

        /// <summary>
        /// Called when the custom firearm damages a player.
        /// </summary>
        /// <param name="_player"></param>
        /// <param name="_target"></param>
        /// <param name="_damage"></param>
        /// <returns></returns>
        public virtual bool Damage(Player _player, Player _target, DamageHandlerBase _damage)
        {
            if (_player == null || _target == null)
                return true;

            if (_damage is FirearmDamageHandler standard)
            {
                if (_player == _target)
                    return false;

                float damage = 0f;
                float hitmarker = 1f;

                switch (standard.Hitbox)
                {
                    case HitboxType.Headshot:
                        hitmarker = 2.5f;
                        damage = Data.HeadDamage;
                        break;
                    case HitboxType.Body:
                        hitmarker = 1f;
                        damage = Data.BodyDamage;
                        break;
                    case HitboxType.Limb:
                        hitmarker = 0.5f;
                        damage = Data.LimbDamage;
                        break;
                }

                if (_target.IsSCP)
                    damage = Data.SCPDamage;

                if ((_player.Role.GetFaction() != _target.Role.GetFaction()) || (Server.FriendlyFire && _player.Role.GetFaction() == _target.Role.GetFaction()))
                {
                    _player.ReceiveHitMarker(hitmarker);

                    if (FirearmDamageHandler.HitboxDamageMultipliers.TryGetValue(standard.Hitbox, out float m))
                        damage /= m;

                    standard.Damage = damage;
                }
                else if (Data.FriendlyAction != FirearmFriendlyAction.None && _target.Role.GetFaction() == _player.Role.GetFaction())
                {
                    _player.ReceiveHitMarker();

                    switch (Data.FriendlyAction) // Needs refactoring to custom friendly action classes
                    {
                        case FirearmFriendlyAction.Heal:
                            damage = Data.FriendlyValue;

                            if (_target.Health < _target.MaxHealth)
                                _target.Heal(damage);

                            _player.ReceiveHint("Healed " + _target.DisplayNickname + ": <color=#00FF00>+" + (int)damage + " HP</color>\nTheir Health: <color=#00FF00>" + (int)_target.Health + "/" + (int)_target.MaxHealth + "</color>", new HintEffect[] { HintEffectPresets.FadeOut() }, 1f);
                            _target.ReceiveHint("Healing From " + _player.DisplayNickname + ": <color=#00FF00>+" + (int)damage + " HP</color>", new HintEffect[] { HintEffectPresets.FadeOut() }, 1f);
                            break;
                        case FirearmFriendlyAction.Speed:
                            damage = Data.FriendlyValue;
                            MovementBoost m = _target.EffectsManager.EnableEffect<MovementBoost>(3f, false);
                            m.Intensity = (byte)damage;
                            m = _player.EffectsManager.EnableEffect<MovementBoost>(3f, false);
                            m.Intensity = (byte)damage;
                            _player.ReceiveHint("Speed Boosted " + _target.DisplayNickname, new HintEffect[] { HintEffectPresets.FadeOut() }, 3f);
                            _target.ReceiveHint("Speed Boost From " + _player.DisplayNickname, new HintEffect[] { HintEffectPresets.FadeOut() }, 3f);
                            break;
                    }
                }

                if (Data.LifeSteal != 0 && (_target.Role != _player.Role) && (_target.Role.GetFaction() != _player.Role.GetFaction() || Server.FriendlyFire || _player.Health >= _player.MaxHealth))
                    _player.Heal(damage * Data.LifeSteal);
            }

            return true;
        }

        /// <summary>
        /// Called when the custom weapon starts to reload.
        /// </summary>
        /// <param name="_player"></param>
        /// <param name="_gun"></param>
        public virtual bool Reload(Player _player, Firearm _gun)
        {
            ResetFirearm(_gun, this);

            if (Data.CannotReload)
                return false;

            return true;
        }

        /// <summary>
        /// Called when the custom weapon starts to unload.
        /// </summary>
        /// <param name="_player"></param>
        /// <param name="_gun"></param>
        public virtual bool Unload(Player _player, Firearm _gun)
        {
            ResetFirearm(_gun, this);

            return true;
        }

        /// <summary>
        /// Called when the custom weapon is aimed.
        /// </summary>
        /// <param name="_player"></param>
        /// <param name="_gun"></param>
        /// <param name="_isAiming"></param>
        public virtual void Aim(Player _player, Firearm _gun, bool _isAiming)
        {
            ResetFirearm(_gun, this);
        }

        /// <summary>
        /// Applies the custom stats to a firearm.
        /// </summary>
        /// <param name="firearm"></param>
        /// <param name="gun"></param>
        public static void ResetFirearm(Firearm firearm, CustomItemFirearm gun)
        {
            firearm.GlobalSettingsPreset.AbsoluteJumpInaccuracy = gun.Data.AirSpread;
            firearm.GlobalSettingsPreset.OverallRunningInaccuracyMultiplier = gun.Data.RunSpread;

            if (firearm is AutomaticFirearm auto)
            {
                auto._baseMaxAmmo = gun.Data.MagazineSize;
                auto.AmmoManagerModule = new AutomaticAmmoManager(auto, gun.Data.MagazineSize, 1, gun.Data.ChamberSize);

                var stats = auto._stats;

                stats.HipInaccuracy = gun.Data.HipSpread;
                stats.BulletInaccuracy = 0f;
                stats.AdsInaccuracy = gun.Data.AimSpread;
                stats.DamageFalloff = gun.Data.DamageFalloff;
                stats.FullDamageDistance = gun.Data.FullDamageDistance;

                auto._stats = stats;

            }
            else if (firearm is Revolver rev)
            {
                rev.AmmoManagerModule = new ClipLoadedInternalMagAmmoManager(rev, gun.Data.MagazineSize);

                var stats = rev._stats;

                stats.HipInaccuracy = gun.Data.HipSpread;
                stats.BulletInaccuracy = 0f;
                stats.AdsInaccuracy = gun.Data.AimSpread;
                stats.DamageFalloff = gun.Data.DamageFalloff;
                stats.FullDamageDistance = gun.Data.FullDamageDistance;

                rev._stats = stats;
            }
            else if (firearm is Shotgun shot)
            {
                shot.AmmoManagerModule = new TubularMagazineAmmoManager(shot, shot.ItemSerial, gun.Data.MagazineSize, gun.Data.ChamberSize, 0.5f, 3, "ShellsToLoad", ActionName.Zoom, ActionName.Shoot, ActionName.InspectItem);

                var stats = shot._stats;

                stats.HipInaccuracy = gun.Data.HipSpread;
                stats.BulletInaccuracy = 0f;
                stats.AdsInaccuracy = gun.Data.AimSpread;
                stats.DamageFalloff = gun.Data.DamageFalloff;
                stats.FullDamageDistance = gun.Data.FullDamageDistance;

                shot._stats = stats;
            }
            else if (firearm is ParticleDisruptor par)
            {
                var stats = par._stats;

                stats.HipInaccuracy = gun.Data.HipSpread;
                stats.BulletInaccuracy = 0f;
                stats.AdsInaccuracy = gun.Data.AimSpread;
                stats.DamageFalloff = gun.Data.DamageFalloff;
                stats.FullDamageDistance = gun.Data.FullDamageDistance;

                par._stats = stats;

                par.Status = new FirearmStatus(gun.Data.MagazineSize, par.Status.Flags, par.Status.Attachments);
            }
        }

        public override void ActionHint(Player _player, string _action)
        {
            _player.ReceiveHint($"{_action}: <b><color=#00FFFF>{DisplayName}</color></b>\nAmmo: <b><color=#FFFF00>{((Firearm)_player.CurrentItem).Status.Ammo}</color>/{((Firearm)_player.CurrentItem).AmmoManagerModule.MaxAmmo}</b>", new HintEffect[] { HintEffectPresets.FadeOut() }, 3f);
        }
    }

    /// <summary>
    /// This struct is used as a holder for firearm stats.
    /// </summary>
    public struct CustomFirearmData
    {
        public float HeadDamage;
        public float BodyDamage;
        public float LimbDamage;
        public float SCPDamage;
        public float LifeSteal;

        public float HipSpread;
        public float AimSpread;
        public float RunSpread;
        public float AirSpread;

        public float FullDamageDistance;
        public float DamageFalloff;

        public float FriendlyValue;

        public byte MagazineSize;
        public byte ChamberSize;

        public int ExtraBulletCount;

        public bool CannotReload;

        public FirearmFriendlyAction FriendlyAction;
    }

    public enum FirearmFriendlyAction
    {
        None,
        Heal,
        Speed
    }
}
