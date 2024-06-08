using PlayerRoles.PlayableScps.Scp049.Zombies;
using PlayerStatsSystem;
using UnityEngine;

namespace SwiftAPI.API.BreakableToys
{
    public class BreakableToyHitbox : MonoBehaviour, IDestructible
    {
        public int Layer = LayerMask.NameToLayer("Hitbox");

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
