using CustomPlayerEffects;
using Hints;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.BasicMessages;
using InventorySystem.Items.Firearms.Modules;
using PlayerRoles;
using PlayerStatsSystem;
using PluginAPI.Core;
using SwiftAPI.API.CustomItems.FriendlyActions;
using SwiftAPI.API.ServerVariables;
using UnityEngine;

namespace SwiftAPI.API.CustomItems
{
    /// <summary>
    /// This class is a base for custom firearms/weapons.
    /// </summary>
    public class CustomItemFirearm : CustomItemEquippable
    {
        public CustomFirearmData HipData
        {
            get => hipData;
            set => hipData = value;
        }
        public CustomFirearmData AimData
        {
            get
            {
                if (aimData == null)
                    return HipData;
                return aimData;
            }
            set => aimData = value;
        }

        public bool InfiniteAmmo;

        private CustomFirearmData aimData;
        private CustomFirearmData hipData;

        private bool delay;

        public override void Equip(Player _player, ushort _itemSerial)
        {
            if (_player.CurrentItem is Firearm f)
            {
                ResetFirearm(f, this);

                int setAmmo = Mathf.Min(HipData.MagazineSize, f.Status.Ammo);
                int giveAmmo = f.Status.Ammo - setAmmo;
                f.Status = new FirearmStatus((byte)setAmmo, f.Status.Flags, f.Status.Attachments);

                if (giveAmmo > 0)
                    _player.AddAmmo(f.AmmoType, (ushort)giveAmmo);
            }

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

            CustomFirearmData data = _gun.AdsModule.ServerAds ? AimData : HipData;

            if (data.ExtraBulletCount > 0)
            {
                delay = true;

                for (int i = 0; i < data.ExtraBulletCount; i++)
                {
                    ResetFirearm(_gun, this);

                    _gun.HitregModule.ClientCalculateHit(out ShotMessage msg);
                    _gun.HitregModule.ServerProcessShot(msg);
                }

                delay = false;
            }

            if (data.OneShot)
                _gun.Status = new FirearmStatus(0, _gun.Status.Flags, _gun.Status.Attachments);

            if (InfiniteAmmo || (ServerVariableManager.TryGetVar(ConstStrings.InfiniteAmmoServerVar, out ServerVariable a) && bool.TryParse(a.Value, out bool b) && b))
                _gun.Status = new FirearmStatus(data.MagazineSize, _gun.Status.Flags, _gun.Status.Attachments);
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
                CustomFirearmData data = ((Firearm)_player.CurrentItem).AdsModule.ServerAds ? AimData : HipData;

                if (_player == _target)
                    return false;

                float damage = 0f;

                switch (standard.Hitbox)
                {
                    case HitboxType.Headshot: damage = data.HeadDamage; break;
                    case HitboxType.Body: damage = data.BodyDamage; break;
                    case HitboxType.Limb: damage = data.LimbDamage; break;
                }

                if (_target.IsSCP)
                    damage = data.SCPDamage;

                if ((_player.Role.GetFaction() != _target.Role.GetFaction()) || (Server.FriendlyFire && _player.Role.GetFaction() == _target.Role.GetFaction()))
                {
                    if (FirearmDamageHandler.HitboxDamageMultipliers.TryGetValue(standard.Hitbox, out float m))
                        damage /= m;

                    standard.Damage = damage;
                }
                else if (data.FriendlyAction != null && _target.Role.GetFaction() == _player.Role.GetFaction())
                {
                    _player.ReceiveHitMarker();

                    data.FriendlyAction.Hit(_player, _target);
                }

                if (data.LifeSteal != 0 && (_target.Role != _player.Role) && (_target.Role.GetFaction() != _player.Role.GetFaction() || Server.FriendlyFire || _player.Health >= _player.MaxHealth))
                    _player.Heal(damage * data.LifeSteal);

                if (data.FirearmEffects != null && data.FirearmEffects.Length > 0)
                    foreach (FirearmEffect eff in data.FirearmEffects)
                        eff.ApplyEffect(_target);

                if (data.HitMessage != null && data.HitMessage.Length > 0)
                    _target.ReceiveHint(data.HitMessage.RandomItem());
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

            ActionHint(_player, "Weapon");

            if (HipData.CannotReload)
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
            ResetFirearm(_gun, this, _isAiming);
        }

        public virtual void StatusChanged(Player _player, Firearm firearm, FirearmStatus prevStatus, FirearmStatus currStatus)
        {
            ActionHint(_player, "Weapon");
        }

        public static void ResetFirearm(Firearm firearm, CustomItemFirearm gun)
        {
            ResetFirearm(firearm, gun, firearm.AdsModule.ServerAds);
        }

        /// <summary>
        /// Applies the custom stats to a firearm.
        /// </summary>
        /// <param name="firearm"></param>
        /// <param name="gun"></param>
        public static void ResetFirearm(Firearm firearm, CustomItemFirearm gun, bool isAiming)
        {
            if (gun.HipData == null)
                Log.Error("ERROR: Custom Firearm \"" + gun.DisplayName + "\" with ID \"" + gun.CustomItemID + "\" requires CustomItemFirearm.HipData to be assigned!");

            CustomFirearmData data = isAiming ? gun.AimData : gun.HipData;

            firearm.GlobalSettingsPreset.AbsoluteJumpInaccuracy = data.AirSpread;
            firearm.GlobalSettingsPreset.OverallRunningInaccuracyMultiplier = data.RunSpread;

            if (firearm is AutomaticFirearm auto)
            {
                auto._baseMaxAmmo = gun.HipData.MagazineSize;
                auto.ActionModule = new AutomaticAction(auto, auto._semiAutomatic, auto._boltTravelTime, 1f / auto._fireRate, auto._dryfireClipId, auto._triggerClipId, auto._gunshotPitchRandomization, auto._recoil, auto._recoilPattern, auto._hasBoltLock, Mathf.Max(1, data.ChamberSize));
                auto.AmmoManagerModule = new AutomaticAmmoManager(auto, gun.HipData.MagazineSize, data.ChamberSize, 0);

                var stats = auto._stats;

                stats.HipInaccuracy = gun.HipData.Spread;
                stats.BulletInaccuracy = 0f;
                stats.AdsInaccuracy = gun.AimData.Spread;
                stats.DamageFalloff = data.DamageFalloff;
                stats.FullDamageDistance = data.FullDamageDistance;

                auto._stats = stats;

            }
            else if (firearm is Revolver rev)
            {
                rev.AmmoManagerModule = new ClipLoadedInternalMagAmmoManager(rev, gun.HipData.MagazineSize);

                var stats = rev._stats;

                stats.HipInaccuracy = gun.HipData.Spread;
                stats.BulletInaccuracy = 0f;
                stats.AdsInaccuracy = gun.AimData.Spread;
                stats.DamageFalloff = data.DamageFalloff;
                stats.FullDamageDistance = data.FullDamageDistance;

                rev._stats = stats;
            }
            else if (firearm is Shotgun shot)
            {
                // shot.AmmoManagerModule = new TubularMagazineAmmoManager(shot, shot.ItemSerial, gun.Data.MagazineSize, shot._numberOfChambers, 0.5f, 3, "ShellsToLoad", ActionName.Zoom, ActionName.Shoot);

                var stats = shot._stats;

                stats.HipInaccuracy = gun.HipData.Spread;
                stats.BulletInaccuracy = 0f;
                stats.AdsInaccuracy = gun.AimData.Spread;
                stats.DamageFalloff = data.DamageFalloff;
                stats.FullDamageDistance = data.FullDamageDistance;

                shot._stats = stats;
            }
            else if (firearm is ParticleDisruptor par)
            {
                var stats = par._stats;

                stats.HipInaccuracy = gun.HipData.Spread;
                stats.BulletInaccuracy = 0f;
                stats.AdsInaccuracy = gun.AimData.Spread;
                stats.DamageFalloff = data.DamageFalloff;
                stats.FullDamageDistance = data.FullDamageDistance;

                par._stats = stats;
            }
        }

        public override void ActionHint(Player _player, string _action)
        {
            if (_player.CurrentItem != null && _player.CurrentItem is Firearm f)
                _player.ReceiveHint($"{_action}: <b><color=#00FFFF>{DisplayName}</color></b>\nAmmo: <b><color=#FFFF00>{(InfiniteAmmo ? "∞" : f.Status.Ammo.ToString())}</color>/{(f is ParticleDisruptor ? HipData.MagazineSize : f.AmmoManagerModule.MaxAmmo)}</b>", new HintEffect[] { HintEffectPresets.FadeOut() }, 3f);
            else
                base.ActionHint(_player, _action);
        }
    }

    /// <summary>
    /// This struct is used as a holder for firearm stats.
    /// </summary>
    public class CustomFirearmData
    {
        public float HeadDamage;
        public float BodyDamage;
        public float LimbDamage;
        public float SCPDamage;
        public float LifeSteal;

        public float Spread;
        public float RunSpread;
        public float AirSpread;

        public float FullDamageDistance;
        public float DamageFalloff;

        /// <summary>
        /// Doesn't work with shotgun currently.
        /// </summary>
        public byte MagazineSize;

        public int ExtraBulletCount;
        public int ChamberSize;

        public bool CannotReload;
        public bool OneShot;

        public FriendlyAction FriendlyAction;

        public FirearmEffect[] FirearmEffects;

        public string[] HitMessage;
    }

    public abstract class FirearmEffect
    {
        public float Duration;
        public bool AddDuration;

        public abstract void ApplyEffect(Player p);
    }

    public class FirearmEffect<T> : FirearmEffect where T : StatusEffectBase
    {
        public override void ApplyEffect(Player p) => p.EffectsManager.EnableEffect<T>(Duration, AddDuration);
    }
}
