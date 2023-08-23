using InventorySystem.Items.Firearms;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomItemAPI.API
{
    /// <summary>
    /// This class is a base for custom firearms/weapons.
    /// </summary>
    public class CustomItemFirearm : CustomItemEquippable
    {
        public CustomFirearmData Data;

        /// <summary>
        /// Called when the custom weapon is shot.
        /// </summary>
        /// <param name="_player"></param>
        /// <param name="_gun"></param>
        public virtual void Shoot(Player _player, Firearm _gun)
        {

        }

        /// <summary>
        /// Called when the custom weapon starts to reload.
        /// </summary>
        /// <param name="_player"></param>
        /// <param name="_gun"></param>
        public virtual void Reload(Player _player, Firearm _gun)
        {

        }

        /// <summary>
        /// Called when the custom weapon starts to unload.
        /// </summary>
        /// <param name="_player"></param>
        /// <param name="_gun"></param>
        public virtual void Unload(Player _player, Firearm _gun)
        {

        }

        /// <summary>
        /// Called when the custom weapon is aimed.
        /// </summary>
        /// <param name="_player"></param>
        /// <param name="_gun"></param>
        /// <param name="_isAiming"></param>
        public virtual void Aim(Player _player, Firearm _gun, bool _isAiming)
        {

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
