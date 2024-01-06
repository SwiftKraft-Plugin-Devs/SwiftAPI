using InventorySystem.Items;
using PluginAPI.Core;

namespace SwiftAPI.API.CustomItems
{
    /// <summary>
    /// This custom class is for custom coins used for coin flip events.
    /// </summary>
    public abstract class CustomItemCoin : CustomItemEquippable
    {
        /// <summary>
        /// Called when the coin has decided if it is tails or heads.
        /// </summary>
        /// <param name="_player"></param>
        /// <param name="_item"></param>
        /// <param name="_tails"></param>
        public abstract void Flip(Player _player, ItemBase _item, bool _tails);
    }
}
