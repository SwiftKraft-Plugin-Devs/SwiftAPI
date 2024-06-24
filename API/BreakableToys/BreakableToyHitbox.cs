using PlayerRoles;
using PlayerStatsSystem;
using PluginAPI.Core;
using SwiftAPI.API.CustomItems;
using UnityEngine;

namespace SwiftAPI.API.BreakableToys
{
    public class BreakableToyHitbox : MonoBehaviour, IDestructible
    {
        public int Layer = LayerMask.NameToLayer("Glass");

        public BreakableToyBase Parent;
        public Collider Collider;

        public uint NetworkId => Parent.NetworkId;

        public Vector3 CenterOfMass => Parent.transform.position;

        private void Awake()
        {
            gameObject.layer = Layer;
        }

        public bool Damage(float damage, DamageHandlerBase handler, Vector3 exactHitPos)
        {
            ReferenceHub attacker = null;
            string[] tags = [];

            if (handler is AttackerDamageHandler atk)
            {
                attacker = atk.Attacker.Hub;
                if (attacker.GetFaction() == Parent.Faction && !Server.FriendlyFire)
                    return false;
            }

            if (attacker != null && attacker.inventory.CurInstance != null && attacker.inventory.CurInstance.ItemSerial.TryGetCustomItemWithSerial(out CustomItemBase cust))
                tags = cust.Tags;

            Parent.Damage(damage, attacker, tags);
            return true;
        }
    }
}
