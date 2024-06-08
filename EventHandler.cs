using InventorySystem.Items.Firearms;
using InventorySystem.Items.ThrowableProjectiles;
using MEC;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;
using SwiftAPI.API.BreakableToys;
using SwiftAPI.API.CustomItems;
using SwiftAPI.Utility.Spawners;
using System.Collections.Generic;
using UnityEngine;

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
            if (!CustomItemManager.IsCustomItem(_event.Firearm.ItemSerial) || CustomItemManager.GetCustomItemWithSerial(_event.Firearm.ItemSerial) is not CustomItemFirearm firearm)
                return;

            firearm.Shoot(_event.Player, _event.Firearm);
        }

        [PluginEvent(ServerEventType.PlayerReloadWeapon)]
        public bool PlayerReloadWeapon(PlayerReloadWeaponEvent _event)
        {
            if (!CustomItemManager.IsCustomItem(_event.Firearm.ItemSerial) || CustomItemManager.GetCustomItemWithSerial(_event.Firearm.ItemSerial) is not CustomItemFirearm firearm)
                return true;

            return firearm.Reload(_event.Player, _event.Firearm);
        }

        [PluginEvent(ServerEventType.PlayerUnloadWeapon)]
        public bool PlayerUnloadWeapon(PlayerUnloadWeaponEvent _event)
        {
            if (!CustomItemManager.IsCustomItem(_event.Firearm.ItemSerial) || CustomItemManager.GetCustomItemWithSerial(_event.Firearm.ItemSerial) is not CustomItemFirearm firearm)
                return true;

            return firearm.Unload(_event.Player, _event.Firearm);
        }

        [PluginEvent(ServerEventType.PlayerAimWeapon)]
        public void PlayerAimWeapon(PlayerAimWeaponEvent _event)
        {
            if (!CustomItemManager.IsCustomItem(_event.Firearm.ItemSerial) || CustomItemManager.GetCustomItemWithSerial(_event.Firearm.ItemSerial) is not CustomItemFirearm firearm)
                return;

            firearm.Aim(_event.Player, _event.Firearm, _event.IsAiming);
        }

        [PluginEvent(ServerEventType.PlayerDamage)]
        public bool PlayerDamage(PlayerDamageEvent _event)
        {
            Player player = _event.Player;
            Player target = _event.Target;

            if (player == null || target == null || player.CurrentItem == null || !CustomItemManager.IsCustomItem(player.CurrentItem.ItemSerial) || CustomItemManager.GetCustomItemWithSerial(player.CurrentItem.ItemSerial) is not CustomItemFirearm firearm)
                return true;

            return firearm.Damage(player, target, _event.DamageHandler);
        }

        [PluginEvent(ServerEventType.PlayerThrowProjectile)]
        public void PlayerThrowProjectile(PlayerThrowProjectileEvent _event)
        {
            if (_event.Item.Projectile is Scp018Projectile proj)
            {
                proj.OnCollided -= OnScp018Collide;
                proj.OnCollided += OnScp018Collide;

                void OnScp018Collide(Collision collision)
                {
                    this.OnScp018Collide(collision, _event.Thrower.ReferenceHub);
                }
            }

            if (!CustomItemManager.IsCustomItem(_event.Item.ItemSerial) || CustomItemManager.GetCustomItemWithSerial(_event.Item.ItemSerial) is not CustomItemThrowableProjectile projectile)
                return;

            projectile.Throw(_event.Thrower, _event.Item, _event.ProjectileSettings);

            if (_event.Item.Projectile is ThrownProjectile pr)
            {
                pr.OnCollided -= OnProjectileCollide;
                pr.OnCollided += OnProjectileCollide;

                void OnProjectileCollide(Collision collision)
                {
                    projectile.Collide(collision);
                }
            }
        }

        [PluginEvent(ServerEventType.GrenadeExploded)]
        public void GrenadeExploded(GrenadeExplodedEvent _event)
        {
            if (_event.Grenade is ExplosionGrenade gre)
                DamageBreakables(_event.Position, gre._maxRadius, 0f, attacker: _event.Thrower.Hub, single: false, damageDrop: gre._playerDamageOverDistance);

            if (!CustomItemManager.IsCustomItem(_event.Grenade.Info.Serial) || CustomItemManager.GetCustomItemWithSerial(_event.Grenade.Info.Serial) is not CustomItemTimeGrenade grenade)
                return;

            grenade.Detonate(_event.Grenade, _event.Position);
        }

        [PluginEvent(ServerEventType.PlayerCoinFlip)]
        public void PlayerCoinFlip(PlayerCoinFlipEvent _event)
        {
            if (!CustomItemManager.IsCustomItem(_event.Player.CurrentItem.ItemSerial) || CustomItemManager.GetCustomItemWithSerial(_event.Player.CurrentItem.ItemSerial) is not CustomItemCoin coin)
                return;

            coin.Flip(_event.Player, _event.Player.CurrentItem, _event.IsTails);
        }

        [PluginEvent(ServerEventType.RoundRestart)]
        public void RoundRestart(RoundRestartEvent _event)
        {
            SpawnerManager.ClearSpawners();
            CustomItemManager.ClearCustomItems();
        }

        [PluginEvent(ServerEventType.Scp096Charging)]
        public void Scp096Charging(Scp096ChargingEvent _event)
        {
            Timing.RunCoroutine(Scp096Break(_event.Player, 2f));
        }

        public IEnumerator<float> Scp096Break(Player p, float duration)
        {
            while (duration > 0f)
            {
                DamageBreakables(p.Position, 3f, 90f);
                duration -= Time.deltaTime;
                yield return Timing.WaitForOneFrame;
            }
        }

        private void OnScp018Collide(Collision collision, ReferenceHub attacker)
        {
            DamageBreakable(collision.collider, 50f, attacker);
        }

        public static void PlaceBulletHoleFirearm(Vector3 position, Firearm firearm)
        {
            float damage = firearm.BaseStats.BaseDamage;

            string[] tags = null;

            if (CustomItemManager.TryGetCustomItemWithSerial(firearm.ItemSerial, out CustomItemBase _item) && _item is CustomItemFirearm f)
            {
                damage = (firearm.AdsModule.ServerAds ? f.AimData : f.HipData).BodyDamage;
                tags = _item.Tags;
            }

            DamageBreakables(position, 0.05f, damage, attacker: firearm.Footprint.Hub, single: true, tags: tags);
        }

        public static void DamageBreakables(Vector3 position, float radius, float damage, ReferenceHub attacker = null, bool single = true, AnimationCurve damageDrop = null, params string[] tags)
        {
            Collider[] colls = Physics.OverlapSphere(position, radius, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            if (colls.Length > 0)
                foreach (Collider col in colls)
                {
                    //DamageBreakable(col, damageDrop == null ? damage : damageDrop.Evaluate(Vector3.Distance(col.ClosestPoint(position), position)), attacker, tags);

                    if (single)
                        break;
                }
        }

        public static void DamageBreakable(Collider col, float damage, ReferenceHub attacker = null, params string[] tags)
        {
            BreakableToyBase b = col.transform.root.GetComponentInChildren<BreakableToyBase>();
            b?.Damage(damage, attacker, tags);
        }
    }
}
