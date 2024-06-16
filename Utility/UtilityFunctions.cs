using Footprinting;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Attachments;
using InventorySystem.Items.Pickups;
using InventorySystem.Items.ThrowableProjectiles;
using Mirror;
using PluginAPI.Core;
using UnityEngine;

namespace SwiftAPI.Utility
{
    public static class UtilityFunctions
    {
        public static ItemBase GiveItem(this Player p, ItemType item)
        {
            ItemBase it = p.AddItem(item);
            if (it is Firearm f)
                f.Status = new(f.AmmoManagerModule.MaxAmmo, FirearmStatusFlags.MagazineInserted, p.GetAttachments(item));
            return it;
        }

        public static uint GetAttachments(this Player p, ItemType it) => AttachmentsServerHandler.PlayerPreferences.TryGetValue(p.ReferenceHub, out var value) && value.TryGetValue(ItemType.GunCOM18, out var value2) ? value2 : AttachmentsUtils.GetRandomAttachmentsCode(it);

        public static void SpawnActive(this ThrowableItem item, Vector3 position, float fuseTime = -1f, Player owner = null)
        {
            if (item.Projectile is not TimeGrenade)
                return;

            TimeGrenade grenade = (TimeGrenade)Object.Instantiate(item.Projectile, position, Quaternion.identity);
            if (fuseTime >= 0)
                grenade._fuseTime = fuseTime;
            grenade.NetworkInfo = new PickupSyncInfo(item.ItemTypeId, item.Weight, item.ItemSerial);
            grenade.PreviousOwner = new Footprint(owner != null ? owner.ReferenceHub : ReferenceHub.HostHub);
            NetworkServer.Spawn(grenade.gameObject);
            grenade.ServerActivate();
        }

        public static ThrowableItem CreateThrowable(this ItemType type, Player player = null) => (player != null ? player.ReferenceHub : ReferenceHub.HostHub).inventory.CreateItemInstance(new ItemIdentifier(type, ItemSerialGenerator.GenerateNext()), false) as ThrowableItem;
    }
}
