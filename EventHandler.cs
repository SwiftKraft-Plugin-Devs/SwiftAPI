using CustomItemAPI.API;
using CustomPlayerEffects;
using Hints;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.BasicMessages;
using PlayerRoles;
using PlayerStatsSystem;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;

namespace CustomItemAPI
{
    public class EventHandler
    {
        [PluginEvent(ServerEventType.PlayerInteractScp330)]
        public void DebugActions(PlayerInteractScp330Event _event)
        {
            if (!Plugin.DebugMode)
                return;

            _event.Player.GiveCustomItem("debug_item");
            _event.Player.GiveCustomItem("debug_gun");
        }

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

        [PluginEvent(ServerEventType.PlayerShotWeapon)]
        public void PlayerShotWeapon(PlayerShotWeaponEvent _event)
        {
            if (!CustomItemManager.IsCustomItem(_event.Firearm.ItemSerial) || !(CustomItemManager.GetCustomItemWithSerial(_event.Firearm.ItemSerial) is CustomItemFirearm firearm))
                return;

            firearm.Shoot(_event.Player, _event.Firearm);
        }

        [PluginEvent(ServerEventType.PlayerReloadWeapon)]
        public bool PlayerReloadWeapon(PlayerReloadWeaponEvent _event)
        {
            if (!CustomItemManager.IsCustomItem(_event.Firearm.ItemSerial) || !(CustomItemManager.GetCustomItemWithSerial(_event.Firearm.ItemSerial) is CustomItemFirearm firearm))
                return true;

            return firearm.Reload(_event.Player, _event.Firearm);
        }

        [PluginEvent(ServerEventType.PlayerUnloadWeapon)]
        public bool PlayerUnloadWeapon(PlayerUnloadWeaponEvent _event)
        {
            if (!CustomItemManager.IsCustomItem(_event.Firearm.ItemSerial) || !(CustomItemManager.GetCustomItemWithSerial(_event.Firearm.ItemSerial) is CustomItemFirearm firearm))
                return true;

            return firearm.Unload(_event.Player, _event.Firearm);
        }

        [PluginEvent(ServerEventType.PlayerAimWeapon)]
        public void PlayerAimWeapon(PlayerAimWeaponEvent _event)
        {
            if (!CustomItemManager.IsCustomItem(_event.Firearm.ItemSerial) || !(CustomItemManager.GetCustomItemWithSerial(_event.Firearm.ItemSerial) is CustomItemFirearm firearm))
                return;

            firearm.Aim(_event.Player, _event.Firearm, _event.IsAiming);
        }

        [PluginEvent(ServerEventType.PlayerDamage)]
        public bool PlayerDamage(PlayerDamageEvent _event)
        {
            Player player = _event.Target;
            Player target = _event.Player;

            if (player == null || target == null || player.CurrentItem == null || !CustomItemManager.IsCustomItem(player.CurrentItem.ItemSerial) || !(CustomItemManager.GetCustomItemWithSerial(player.CurrentItem.ItemSerial) is CustomItemFirearm firearm))
                return true;

            return firearm.Damage(player, target, _event.DamageHandler);
        }

        [PluginEvent(ServerEventType.PlayerShotWeapon)]
        public void OnShotWeapon(PlayerShotWeaponEvent _event)
        {
            if (!CustomItemManager.IsCustomItem(_event.Firearm.ItemSerial) || !(CustomItemManager.GetCustomItemWithSerial(_event.Firearm.ItemSerial) is CustomItemFirearm gun))
                return;

            gun.Shoot(_event.Player, _event.Firearm);
        }
    }
}
