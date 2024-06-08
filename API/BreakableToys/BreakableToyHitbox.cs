using PlayerStatsSystem;
using UnityEngine;

namespace SwiftAPI.API.BreakableToys
{
    public class BreakableToyHitbox : MonoBehaviour, IDestructible
    {
        public const int Layer = 3;

        public BreakableToyBase Parent;

        public uint NetworkId => Parent.NetworkId;

        public Vector3 CenterOfMass => Parent.transform.position;

        private void Awake()
        {
            gameObject.layer = Layer;
        }

        public bool Damage(float damage, DamageHandlerBase handler, Vector3 exactHitPos)
        {
            Parent.Damage(damage);
            return true;
        }
    }
}
