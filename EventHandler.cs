using CustomItemAPI.API;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomItemAPI
{
    public class EventHandler
    {
        [PluginEvent(ServerEventType.PlayerSearchPickup)]
        public void PlayerSearchPickup(PlayerSearchPickupEvent _event)
        {
            if (!CustomItemManager.IsCustomItem(_event.Item.Info.Serial))
                return;

            CustomItemBase item = CustomItemManager.GetCustomItemWithSerial(_event.Item.Info.Serial);
            item.Pickup(_event.Player, _event.Item);
        }

        [PluginEvent(ServerEventType.PlayerDropItem)]
        public void PlayerDropItem(PlayerDropItemEvent _event)
        {
            if (!CustomItemManager.IsCustomItem(_event.Item.ItemSerial))
                return;

            CustomItemBase item = CustomItemManager.GetCustomItemWithSerial(_event.Item.ItemSerial);
            item.Drop(_event.Player, _event.Item);
        }

        [PluginEvent(ServerEventType.PlayerChangeItem)]
        public void PlayerChangeItem(PlayerChangeItemEvent _event)
        {
            if (CustomItemManager.IsCustomItem(_event.NewItem))
            {
                CustomItemBase item = CustomItemManager.GetCustomItemWithSerial(_event.NewItem);
                if (item is CustomItemEquippable equippable)
                    equippable.Equip(_event.Player, _event.NewItem);
            }

            if (CustomItemManager.IsCustomItem(_event.OldItem))
            {
                CustomItemBase itemOld = CustomItemManager.GetCustomItemWithSerial(_event.OldItem);
                if (itemOld is CustomItemEquippable equippableOld)
                    equippableOld.Unequip(_event.Player, _event.OldItem);
            }
        }
    }
}
