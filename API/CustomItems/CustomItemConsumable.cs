using InventorySystem.Items;
using PluginAPI.Core;

namespace SwiftAPI.API.CustomItems
{
    public class CustomItemConsumable : CustomItemEquippable
    {
        public GeneralEffect[] Effects;

        /// <summary>
        /// Called when started consuming the custom item.
        /// </summary>
        /// <param name="_player"></param>
        /// <param name="_item"></param>
        public virtual void StartConsume(Player _player, ItemBase _item)
        {
            ActionHint(_player, "Using");
        }

        /// <summary>
        /// Called when canceled consuming the custom item.
        /// </summary>
        /// <param name="_player"></param>
        /// <param name="_item"></param>
        public virtual void CancelConsume(Player _player, ItemBase _item)
        {
            ActionHint(_player, "Canceled");
        }

        /// <summary>
        /// Called when consumed the custom item.
        /// </summary>
        /// <param name="_player"></param>
        /// <param name="_item"></param>
        public virtual void EndConsume(Player _player, ItemBase _item)
        {
            CustomItemManager.RemoveCustomItem(_item.ItemSerial);
            if (Effects != null && Effects.Length > 0)
                foreach (GeneralEffect e in Effects)
                    e.ApplyEffect(_player);
            ActionHint(_player, "Used");
        }
    }
}
