using Hints;
using InventorySystem.Items;
using InventorySystem.Items.Pickups;
using PluginAPI.Core;
using System.Collections.Generic;

namespace SwiftAPI.API
{
    /// <summary>
    /// Base class for custom items.
    /// Can inherit for different functionalities.
    /// This class serves as an identifier for a custom item, 
    /// if you want to make custom states for custom items, 
    /// you can modify the custom status.
    /// </summary>
    public abstract class CustomItemBase
    {
        public readonly Dictionary<ushort, CustomItemStatusBase> ItemStatuses = new Dictionary<ushort, CustomItemStatusBase>();

        public string CustomItemID;
        public string DisplayName;
        public string Description;

        public string[] Tags;

        public bool Undroppable;

        public ItemType BaseItem;

        /// <summary>
        /// Called when picking up the item.
        /// </summary>
        /// <param name="_player"></param>
        /// <param name="_itemPickupBase"></param>
        public virtual bool StartPickup(Player _player, ItemPickupBase _itemPickupBase)
        {
            ActionHint(_player, "Picking Up");
            return true;
        }

        /// <summary>
        /// Called when picked up the item.
        /// </summary>
        /// <param name="_player"></param>
        /// <param name="_itemPickupBase"></param>
        public virtual bool EndPickup(Player _player, ItemPickupBase _itemPickupBase)
        {
            ActionHint(_player, "Picked Up");
            return true;
        }

        /// <summary>
        /// Called when dropped the item.
        /// </summary>
        /// <param name="_player"></param>
        /// <param name="_item"></param>
        public virtual bool Drop(Player _player, ItemBase _item)
        {
            ActionHint(_player, Undroppable ? "Unable To Drop" : "Dropped");

            return !Undroppable;
        }

        /// <summary>
        /// Called when the custom item is created.
        /// This will be called before the dictionary is updated.
        /// </summary>
        /// <param name="_itemSerial"></param>
        public abstract void Init(ushort _itemSerial);

        /// <summary>
        /// Called when removed from the tracker dictionary.
        /// This will be called before the dictionary is updated.
        /// </summary>
        /// <param name="_itemSerial"></param>
        public abstract void Destroy(ushort _itemSerial);

        /// <summary>
        /// Gets the status of the custom item that is bound to the item serial. 
        /// </summary>
        /// <param name="_itemSerial"></param>
        /// <returns></returns>
        public CustomItemStatusBase GetStatus(ushort _itemSerial)
        {
            if (ItemStatuses.ContainsKey(_itemSerial))
                return ItemStatuses[_itemSerial];

            return null;
        }

        /// <summary>
        /// Sets the custom status for a custom item.
        /// </summary>
        /// <param name="_itemSerial"></param>
        /// <param name="_status"></param>
        /// <returns></returns>
        public bool SetCustomStatus(ushort _itemSerial, CustomItemStatusBase _status)
        {
            if (!CustomItemManager.IsCustomItem(_itemSerial))
                return false;

            if (ItemStatuses.ContainsKey(_itemSerial))
                ItemStatuses[_itemSerial] = _status;
            else
                ItemStatuses.Add(_itemSerial, _status);

            return true;
        }

        /// <summary>
        /// Removes the custom status from a custom item.
        /// </summary>
        /// <param name="_itemSerial"></param>
        /// <returns></returns>
        public bool RemoveCustomStatus(ushort _itemSerial)
        {
            if (ItemStatuses.ContainsKey(_itemSerial))
            {
                ItemStatuses.Remove(_itemSerial);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the status of the custom item that is bound to the item serial.
        /// </summary>
        /// <param name="_itemSerial"></param>
        /// <param name="_status"></param>
        /// <returns></returns>
        public bool TryGetStatus(ushort _itemSerial, out CustomItemStatusBase _status)
        {
            if (ItemStatuses.ContainsKey(_itemSerial))
            {
                _status = ItemStatuses[_itemSerial];

                return true;
            }

            _status = null;

            return false;
        }

        public virtual void ActionHint(Player _player, string _action)
        {
            _player.ReceiveHint($"{_action}: <b><color=#00FFFF>{DisplayName}</color></b>", new HintEffect[] { HintEffectPresets.FadeOut() }, 3f);
        }
    }

    /// <summary>
    /// Empty class for generic storage of custom item statuses.
    /// You can inherit to write your own logic here.
    /// </summary>
    public abstract class CustomItemStatusBase { }
}
