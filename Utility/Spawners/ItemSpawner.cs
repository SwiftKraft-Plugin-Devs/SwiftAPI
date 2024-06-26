﻿using PluginAPI.Core.Items;
using SwiftAPI.API.CustomItems;
using System;
using UnityEngine;

namespace SwiftAPI.Utility.Spawners
{
    public class ItemSpawner : SpawnerBase
    {
        public ItemType Item;

        public CustomItemBase CustomItem;

        public override void Spawn()
        {
            CreateItem();
        }

        public void CreateItem()
        {
            if (CustomItem == null)
            {
                ItemPickup ib = ItemPickup.Create(Item, Position, Quaternion.identity);
                ib.Spawn();
            }
            else
                CustomItemManager.DropCustomItem(CustomItem, Position);
        }

        public override bool SetSpawnee(string[] value, out string feedback)
        {
            if (int.TryParse(value[0], out int result))
            {
                if (Enum.GetValues(typeof(ItemType)).ToArray<ItemType>().Contains((ItemType)result))
                {
                    Item = (ItemType)result;

                    feedback = "Regular Item Spawner: " + ToString();

                    return true;
                }
                else
                {
                    feedback = $"Regular item with ID {result} doesn't exist! ";

                    return false;
                }
            }
            else if (CustomItemManager.TryGetCustomItemWithID(value[0], out CustomItemBase it))
            {
                Item = it.BaseItem;
                CustomItem = it;

                feedback = "Custom Item Spawner: " + ToString();

                return true;
            }
            else
            {
                feedback = $"Invalid item \"{value}\"! ";

                return false;
            }
        }

        public override string ToString() => base.ToString() + (CustomItem == null ? "\nItem (Regular): " + Item.ToString() + " (" + (int)Item + ")" : "\nItem (Custom): " + CustomItem.DisplayName + " (" + CustomItem.CustomItemID + ")");
    }
}
