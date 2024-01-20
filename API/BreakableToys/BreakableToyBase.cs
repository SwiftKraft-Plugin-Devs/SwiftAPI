using AdminToys;
using Mirror;
using PluginAPI.Core;
using UnityEngine;

namespace SwiftAPI.API.BreakableToys
{
    public class BreakableToyBase : MonoBehaviour
    {
        public PrimitiveObjectToy Toy;

        public float MaxHealth;

        protected float CurrentHealth;

        public uint NetworkId => Toy.netId;

        public void Configure(float max)
        {
            MaxHealth = max;
            CurrentHealth = max;
        }

        public virtual void Damage(float damage)
        {
            CurrentHealth -= damage;

            if (CurrentHealth <= 0f)
                Destroy();
        }

        public virtual void Destroy()
        {
            NetworkServer.Destroy(Toy.gameObject);
        }
    }
}
