using System.Collections.Generic;

namespace CustomItemAPI.API
{
    public static class CustomItemTracker
    {
        // Track the custom items, other plugins should be able to access this.

        public static readonly Dictionary<ushort, CustomItem> Items = new Dictionary<ushort, CustomItem>();
        public static readonly Dictionary<string, CustomItem> RegisteredItems = new Dictionary<string, CustomItem>();

        public static bool AddCustomItem(ushort _itemSerial, CustomItem _item)
        {
            if (_item != null && RegisteredItems.ContainsValue(_item))
            {
                Items.Add(_itemSerial, _item);
                return true;
            }

            return false;
        }

        public static bool AddCustomItem(ushort _itemSerial, string _itemId)
        {
            return AddCustomItem(_itemSerial, GetCustomItemWithID(_itemId));
        }

        public static bool RemoveCustomItem(ushort _itemSerial)
        {
            if (Items.ContainsKey(_itemSerial))
            {
                Items.Remove(_itemSerial);
                return true;
            }

            return false;
        }

        public static void ClearCustomItems()
        {
            Items.Clear();
        }

        public static bool RegisterItem(string _id, CustomItem _item)
        {
            if (!RegisteredItems.ContainsKey(_id))
            {
                RegisteredItems.Add(_id, _item);
                return true;
            }

            return false;
        }

        public static bool UnregisterItem(string _id)
        {
            if (RegisteredItems.ContainsKey(_id))
            {
                RegisteredItems.Remove(_id);
                return true;
            }

            return false;
        }

        public static CustomItem GetCustomItemWithID(string _id)
        {
            if (RegisteredItems.ContainsKey(_id))
                return RegisteredItems[_id];

            return null;
        }

        public static bool IsCustomItem(ushort _itemSerial)
        {
            return Items.ContainsKey(_itemSerial);
        }
    }
}
