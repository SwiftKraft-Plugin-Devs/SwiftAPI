using CustomPlayerEffects;
using Hints;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.BasicMessages;
using PlayerRoles;
using PlayerStatsSystem;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomItemAPI.API
{
    /// <summary>
    /// This class is a base for custom firearms/weapons.
    /// </summary>
    public class CustomItemFirearm : CustomItemEquippable
    {
        public CustomFirearmData Data;

        bool delay;

        /// <summary>
        /// Called when the custom weapon is shot.
        /// </summary>
        /// <param name="_player"></param>
        /// <param name="_gun"></param>
        public virtual void Shoot(Player _player, Firearm _gun)
        {
            if (delay)
                return;

            CustomItemFirearm gun = (CustomItemFirearm)CustomItemManager.GetCustomItemWithSerial(_gun.ItemSerial);

            if (gun == null)
                return;

            ResetFirearm(_gun, gun);

            if (gun.Data.ExtraBulletCount > 0)
            {
                delay = true;

                for (int i = 0; i < gun.Data.ExtraBulletCount; i++)
                {
                    ResetFirearm(_gun, gun);

                    _gun.HitregModule.ClientCalculateHit(out ShotMessage msg);
                    _gun.HitregModule.ServerProcessShot(msg);
                }

                delay = false;
            }
        }

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

                CustomItemFirearm gun = (CustomItemFirearm)CustomItemManager.GetCustomItemWithSerial(_player.CurrentItem.ItemSerial);

                switch (standard.Hitbox)
                {
                    case HitboxType.Headshot:
                        hitmarker = 2.5f;
                        damage = gun.Data.HeadDamage;
                        break;
                    case HitboxType.Body:
                        hitmarker = 1f;
                        damage = gun.Data.BodyDamage;
                        break;
                    case HitboxType.Limb:
                        hitmarker = 0.5f;
                        damage = gun.Data.LimbDamage;
                        break;
                }

                if (_target.IsSCP)
                    damage = gun.Data.SCPDamage;

                if ((_player.Role.GetFaction() != _target.Role.GetFaction()) || (Server.FriendlyFire && _player.Role.GetFaction() == _target.Role.GetFaction()))
                {
                    _player.ReceiveHitMarker(hitmarker);

                    if (FirearmDamageHandler.HitboxDamageMultipliers.TryGetValue(standard.Hitbox, out float m))
                        damage /= m;

                    standard.Damage = damage;
                }
                else if (gun.Data.FriendlyAction != FirearmFriendlyAction.None && _target.Role.GetFaction() == _player.Role.GetFaction())
                {
                    _player.ReceiveHitMarker();

                    switch (gun.Data.FriendlyAction)
                    {
                        case FirearmFriendlyAction.Heal:
                            damage = gun.Data.FriendlyValue;

                            if (_target.Health < _target.MaxHealth)
                                _target.Heal(damage);

                            _player.ReceiveHint("Healed " + _target.DisplayNickname + ": <color=#00FF00>+" + (int)damage + " HP</color>\nTheir Health: <color=#00FF00>" + (int)_target.Health + "/" + (int)_target.MaxHealth + "</color>", new HintEffect[] { HintEffectPresets.FadeOut() }, 1f);
                            _target.ReceiveHint("Healing From " + _player.DisplayNickname + ": <color=#00FF00>+" + (int)damage + " HP</color>", new HintEffect[] { HintEffectPresets.FadeOut() }, 1f);
                            break;
                        case FirearmFriendlyAction.Speed:
                            damage = gun.Data.FriendlyValue;
                            MovementBoost m = _target.EffectsManager.EnableEffect<MovementBoost>(3f, false);
                            m.Intensity = (byte)damage;
                            m = _player.EffectsManager.EnableEffect<MovementBoost>(3f, false);
                            m.Intensity = (byte)damage;
                            _player.ReceiveHint("Speed Boosted " + _target.DisplayNickname, new HintEffect[] { HintEffectPresets.FadeOut() }, 3f);
                            _target.ReceiveHint("Speed Boost From " + _player.DisplayNickname, new HintEffect[] { HintEffectPresets.FadeOut() }, 3f);
                            break;
                    }
                }

                if (gun.Data.LifeSteal != 0 && (_target.Role != _player.Role) && (_target.Role.GetFaction() != _player.Role.GetFaction() || Server.FriendlyFire || _player.Health >= _player.MaxHealth))
                    _player.Heal(damage * gun.Data.LifeSteal);
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
            CustomItemFirearm gun = (CustomItemFirearm)CustomItemManager.GetCustomItemWithSerial(_gun.ItemSerial);

            if (_gun.Status.Ammo == gun.Data.MagazineSize)
                return false;

            _gun.Status = new FirearmStatus((byte)Mathf.Min(_gun.Status.Ammo, gun.Data.MagazineSize), _gun.Status.Flags, _gun.Status.Attachments);

            void Status(FirearmStatus a, FirearmStatus b)
            {
                if (b.Ammo > gun.Data.MagazineSize)
                {
                    _gun.Status = new FirearmStatus((byte)gun.Data.MagazineSize, _gun.Status.Flags, _gun.Status.Attachments);
                }
            }

            _gun.OnStatusChanged += Status;

            return true;
        }

        /// <summary>
        /// Called when the custom weapon starts to unload.
        /// </summary>
        /// <param name="_player"></param>
        /// <param name="_gun"></param>
        public virtual bool Unload(Player _player, Firearm _gun)
        {
            return true;
        }

        /// <summary>
        /// Called when the custom weapon is aimed.
        /// </summary>
        /// <param name="_player"></param>
        /// <param name="_gun"></param>
        /// <param name="_isAiming"></param>
        public virtual void Aim(Player _player, Firearm _gun, bool _isAiming) { }

        public static void ResetFirearm(Firearm firearm, CustomItemFirearm gun)
        {
            firearm.GlobalSettingsPreset.AbsoluteJumpInaccuracy = gun.Data.AirSpread;
            firearm.GlobalSettingsPreset.OverallRunningInaccuracyMultiplier = gun.Data.RunSpread;

            if (firearm is AutomaticFirearm auto)
            {
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
            }
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

        /// <summary>
        /// This value is limited by the in-game ammo limit. 
        /// </summary>
        public int MagazineSize;
        public int ExtraBulletCount;

        public FirearmFriendlyAction FriendlyAction;
    }

    public enum FirearmFriendlyAction
    {
        None,
        Heal,
        Speed
    }
}
