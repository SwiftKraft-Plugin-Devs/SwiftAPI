using InventorySystem.Items.ThrowableProjectiles;
using PluginAPI.Core;

namespace SwiftAPI.API
{
    /// <summary>
    /// This class is for custom throwables.
    /// </summary>
    public abstract class CustomItemThrowableProjectile : CustomItemEquippable
    {
        /// <summary>
        /// Called when thrown the projectile.
        /// </summary>
        /// <param name="_player"></param>
        /// <param name="_item"></param>
        /// <param name="_projectileSettings"></param>
        public abstract void Throw(Player _player, ThrowableItem _item, ThrowableItem.ProjectileSettings _projectileSettings);
    }
}
