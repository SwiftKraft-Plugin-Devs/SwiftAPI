using AdminToys;
using Mirror;
using PluginAPI.Core.Items;
using SwiftAPI.API.CustomItems;
using UnityEngine;

namespace SwiftAPI.API.BreakableToys
{
    public class BreakableToyBase : MonoBehaviour
    {
        public PrimitiveObjectToy Toy;

        public float MaxHealth;

        protected float CurrentHealth;

        public ItemType DropItem;
        public CustomItemBase DropCustomItem;

        public uint NetworkId => Toy.netId;

        bool dead;

        public virtual void SetHealth(float max)
        {
            MaxHealth = max;
            CurrentHealth = max;
        }

        public virtual void Damage(float damage)
        {
            if (MaxHealth < 0f)
                return;

            CurrentHealth -= damage;

            if (CurrentHealth <= 0f)
                Destroy();
        }

        public virtual void Destroy()
        {
            if (dead)
                return;

            Drop();

            dead = true;

            BreakableToyManager.Breakables.Remove(this);

            NetworkServer.Destroy(Toy.gameObject);
        }

        public void Drop()
        {
            if (DropCustomItem == null && DropItem != ItemType.None)
            {
                ItemPickup ib = ItemPickup.Create(DropItem, Toy.NetworkPosition, Quaternion.identity);
                ib.Spawn();
            }
            else
                CustomItemManager.DropCustomItem(DropCustomItem, Toy.NetworkPosition);
        }
    }
}
