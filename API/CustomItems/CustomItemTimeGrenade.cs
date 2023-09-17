using InventorySystem.Items.Pickups;
using InventorySystem.Items.ThrowableProjectiles;
using PluginAPI.Core;
using UnityEngine;

namespace CustomItemAPI.API
{
    /// <summary>
    /// This class is for custom time grenades.
    /// </summary>
    public class CustomItemTimeGrenade : CustomItemThrowableProjectile
    {
        public CustomGrenadeData Data;

        public override void Throw(Player _player, ThrowableItem _item, ThrowableItem.ProjectileSettings _projectileSettings)
        {
            if (_item.Projectile is TimeGrenade grenade)
                grenade._fuseTime = Data.FuseTime;
        }

        /// <summary>
        /// Called when the grenade detonates.
        /// </summary>
        /// <param name="_grenade"></param>
        /// <param name="_position"></param>
        public virtual void Detonate(ItemPickupBase _grenade, Vector3 _position) { }
    }

    /// <summary>
    /// This struct is used as a holder for timed grenade stats.
    /// </summary>
    public struct CustomGrenadeData
    {
        public float FuseTime;
    }
}
