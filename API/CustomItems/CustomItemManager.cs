using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using PluginAPI.Core;
using PluginAPI.Core.Items;
using System.Collections.Generic;
using UnityEngine;

namespace SwiftAPI.API.CustomItems
{
    /// <summary>
    /// This class is used to track the custom items.
    /// </summary>
    public static class CustomItemManager
    {
        /// <summary>
        /// Dictionary for tracking custom items using item serials. 
        /// Will be referred as "tracker dictionary" in other summaries.
        /// </summary>
        public static readonly Dictionary<ushort, CustomItemBase> Items = new Dictionary<ushort, CustomItemBase>();
        /// <summary>
        /// Dictionary for registering items.
        /// Keeps track of available custom item types.
        /// </summary>
        public static readonly SortedDictionary<string, CustomItemBase> RegisteredItems = new SortedDictionary<string, CustomItemBase>();

        /// <summary>
        /// Adds a custom item to the tracker dictionary.
        /// </summary>
        /// <param name="_itemSerial"></param>
        /// <param name="_item"></param>
        /// <returns></returns>
        public static bool AddCustomItem(ushort _itemSerial, CustomItemBase _item)
        {
            if (_item != null && RegisteredItems.ContainsValue(_item))
            {
                if (Items.ContainsKey(_itemSerial))
                    Items.Remove(_itemSerial);

                _item.Init(_itemSerial);
                Items.Add(_itemSerial, _item);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds a custom item to the tracker dictionary, but uses custom item ID instead of the class.
        /// </summary>
        /// <param name="_itemSerial"></param>
        /// <param name="_itemId"></param>
        /// <returns></returns>
        public static bool AddCustomItem(ushort _itemSerial, string _itemId)
        {
            return AddCustomItem(_itemSerial, GetCustomItemWithID(_itemId));
        }

        /// <summary>
        /// Drops a custom item in a world position.
        /// </summary>
        /// <param name="cust"></param>
        /// <param name="pos"></param>
        /// <param name="rot"></param>
        /// <returns></returns>
        public static bool DropCustomItem(CustomItemBase cust, Vector3 pos, Quaternion rot)
        {
            if (cust == null)
                return false;

            ItemPickup it = ItemPickup.Create(cust.BaseItem, pos, rot);
            it.Spawn();
            return AddCustomItem(it.Serial, cust);
        }

        public static bool DropCustomItem(CustomItemBase cust, Vector3 pos)
        {
            return DropCustomItem(cust, pos, Quaternion.identity);
        }

        /// <summary>
        /// Converts a custom item back to normal.
        /// </summary>
        /// <param name="_itemSerial"></param>
        /// <returns></returns>
        public static bool RemoveCustomItem(ushort _itemSerial)
        {
            if (Items.ContainsKey(_itemSerial))
            {
                Items[_itemSerial].Destroy(_itemSerial);
                Items.Remove(_itemSerial);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Converts all of the custom items back to normal.
        /// </summary>
        public static void ClearCustomItems()
        {
            Items.Clear();
        }

        /// <summary>
        /// Registers a custom item to allow it to be tracked and given to players.
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_item"></param>
        /// <returns></returns>
        public static bool RegisterItem(string _id, CustomItemBase _item)
        {
            if (!RegisteredItems.ContainsKey(_id))
            {
                RegisteredItems.Add(_id, _item);
                _item.CustomItemID = _id;
                Log.Info($"Registered Custom Item: {_item.DisplayName}, With ID: {_id}");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the custom item class from the custom item id. 
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public static CustomItemBase GetCustomItemWithID(string _id)
        {
            if (IsRegistered(_id))
                return RegisteredItems[_id];

            return null;
        }

        /// <summary>
        /// Gets the custom item class from the custom item id. 
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_item"></param>
        /// <returns></returns>
        public static bool TryGetCustomItemWithID(string _id, out CustomItemBase _item)
        {
            if (IsRegistered(_id))
            {
                _item = RegisteredItems[_id];

                return true;
            }

            _item = null;

            return false;
        }

        /// <summary>
        /// Gets the custom item identifier of the item serial.
        /// </summary>
        /// <param name="_itemSerial"></param>
        /// <returns></returns>
        public static CustomItemBase GetCustomItemWithSerial(ushort _itemSerial)
        {
            if (IsCustomItem(_itemSerial))
                return Items[_itemSerial];

            return null;
        }

        /// <summary>
        /// Gets the custom item identifier of the item serial.
        /// </summary>
        /// <param name="_itemSerial"></param>
        /// <param name="_item"></param>
        /// <returns></returns>
        public static bool TryGetCustomItemWithSerial(ushort _itemSerial, out CustomItemBase _item)
        {
            if (IsCustomItem(_itemSerial))
            {
                _item = Items[_itemSerial];

                return true;
            }

            _item = null;

            return false;
        }

        /// <summary>
        /// Checks if the item is a custom item. 
        /// </summary>
        /// <param name="_itemSerial"></param>
        /// <returns></returns>
        public static bool IsCustomItem(ushort _itemSerial)
        {
            return Items.ContainsKey(_itemSerial);
        }

        /// <summary>
        /// Checks if the custom item id is registered.
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public static bool IsRegistered(string _id)
        {
            return RegisteredItems.ContainsKey(_id);
        }

        public static void GiveCustomItem(this Player _player, CustomItemBase _item)
        {
            if (_player.IsInventoryFull)
                return;

            ItemBase _it = _player.AddItem(_item.BaseItem);

            if (_it is Firearm f)
            {
                byte mag;

                if (_item is CustomItemFirearm cf)
                    mag = cf.HipData.MagazineSize;
                else
                    mag = f.AmmoManagerModule.MaxAmmo;

                f.Status = new FirearmStatus(mag, FirearmStatusFlags.MagazineInserted, f.Status.Attachments);
            }

            AddCustomItem(_it.ItemSerial, _item);
        }

        public static void GiveCustomItem(this Player _player, string _itemId)
        {
            _player.GiveCustomItem(GetCustomItemWithID(_itemId));
        }
    }
}
