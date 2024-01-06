using SwiftAPI.API.CustomItems;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;

namespace SwiftAPI
{
    public class EventHandler
    {
        [PluginEvent(ServerEventType.PlayerSearchPickup)]
        public bool PlayerSearchPickup(PlayerSearchPickupEvent _event)
        {
            if (!CustomItemManager.IsCustomItem(_event.Item.Info.Serial))
                return true;

            CustomItemBase item = CustomItemManager.GetCustomItemWithSerial(_event.Item.Info.Serial);
            return item.StartPickup(_event.Player, _event.Item);
        }

        [PluginEvent(ServerEventType.PlayerSearchedPickup)]
        public bool PlayerSearchedPickup(PlayerSearchedPickupEvent _event)
        {
            if (!CustomItemManager.IsCustomItem(_event.Item.Info.Serial))
                return true;

            CustomItemBase item = CustomItemManager.GetCustomItemWithSerial(_event.Item.Info.Serial);
            return item.EndPickup(_event.Player, _event.Item);
        }

        [PluginEvent(ServerEventType.PlayerUsedItem)]
        public void PlayerUsedItem(PlayerUsedItemEvent _event)
        {
            if (!CustomItemManager.IsCustomItem(_event.Item.ItemSerial))
                return;

            CustomItemBase item = CustomItemManager.GetCustomItemWithSerial(_event.Item.ItemSerial);

            if (item is CustomItemConsumable consumable)
                consumable.EndConsume(_event.Player, _event.Item);
        }

        [PluginEvent(ServerEventType.PlayerCancelUsingItem)]
        public void PlayerCancelUsingItem(PlayerCancelUsingItemEvent _event)
        {
            if (!CustomItemManager.IsCustomItem(_event.Item.ItemSerial))
                return;

            CustomItemBase item = CustomItemManager.GetCustomItemWithSerial(_event.Item.ItemSerial);

            if (item is CustomItemConsumable consumable)
                consumable.CancelConsume(_event.Player, _event.Item);
        }

        [PluginEvent(ServerEventType.PlayerUseItem)]
        public void PlayerUseItem(PlayerUseItemEvent _event)
        {
            if (!CustomItemManager.IsCustomItem(_event.Item.ItemSerial))
                return;

            CustomItemBase item = CustomItemManager.GetCustomItemWithSerial(_event.Item.ItemSerial);

            if (item is CustomItemConsumable consumable)
                consumable.StartConsume(_event.Player, _event.Item);
        }

        [PluginEvent(ServerEventType.PlayerDropItem)]
        public bool PlayerDropItem(PlayerDropItemEvent _event)
        {
            if (!CustomItemManager.IsCustomItem(_event.Item.ItemSerial))
                return true;

            CustomItemBase item = CustomItemManager.GetCustomItemWithSerial(_event.Item.ItemSerial);
            return item.Drop(_event.Player, _event.Item);
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
            Player player = _event.Player;
            Player target = _event.Target;

            if (player == null || target == null || player.CurrentItem == null || !CustomItemManager.IsCustomItem(player.CurrentItem.ItemSerial) || !(CustomItemManager.GetCustomItemWithSerial(player.CurrentItem.ItemSerial) is CustomItemFirearm firearm))
                return true;

            return firearm.Damage(player, target, _event.DamageHandler);
        }

        [PluginEvent(ServerEventType.PlayerThrowProjectile)]
        public void PlayerThrowProjectile(PlayerThrowProjectileEvent _event)
        {
            if (!CustomItemManager.IsCustomItem(_event.Item.ItemSerial) || !(CustomItemManager.GetCustomItemWithSerial(_event.Item.ItemSerial) is CustomItemThrowableProjectile projectile))
                return;

            projectile.Throw(_event.Thrower, _event.Item, _event.ProjectileSettings);
        }

        [PluginEvent(ServerEventType.GrenadeExploded)]
        public void GrenadeExploded(GrenadeExplodedEvent _event)
        {
            if (!CustomItemManager.IsCustomItem(_event.Grenade.Info.Serial) || !(CustomItemManager.GetCustomItemWithSerial(_event.Grenade.Info.Serial) is CustomItemTimeGrenade grenade))
                return;

            grenade.Detonate(_event.Grenade, _event.Position);
        }

        [PluginEvent(ServerEventType.PlayerCoinFlip)]
        public void PlayerCoinFlip(PlayerCoinFlipEvent _event)
        {
            if (!CustomItemManager.IsCustomItem(_event.Player.CurrentItem.ItemSerial) || !(CustomItemManager.GetCustomItemWithSerial(_event.Player.CurrentItem.ItemSerial) is CustomItemCoin coin))
                return;

            coin.Flip(_event.Player, _event.Player.CurrentItem, _event.IsTails);
        }
    }
}
