using AdminToys;
using Mirror;
using PluginAPI.Core;
using PluginAPI.Core.Items;
using SwiftAPI.API.CustomItems;
using SwiftAPI.Utility.Misc;
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

        public bool IsMoving => Mover != null && MoveMode != SnappingModes.DontMove;

        bool dead;

        SnappingModes MoveMode = SnappingModes.DontMove;

        Player Mover;

        private void FixedUpdate()
        {
            if (!IsMoving)
            {
                Toy.IsStatic = true;
                return;
            }

            Toy.IsStatic = false;

            Vector3 targetPos = Mover.ReferenceHub.transform.position + Mover.ReferenceHub.transform.forward * 2f;

            switch (MoveMode)
            {
                case SnappingModes.None:
                    Toy.NetworkPosition = targetPos;
                    Toy.NetworkRotation = new LowPrecisionQuaternion(Quaternion.Euler(Mover.Rotation));
                    break;
                case SnappingModes.Grid:
                    Toy.NetworkPosition = GridSnapper.SnapToGrid(targetPos, Toy.NetworkScale, Vector3.one / 4f);
                    break;
                case SnappingModes.NoRot:
                    Toy.NetworkPosition = targetPos;
                    Toy.NetworkRotation = new LowPrecisionQuaternion();
                    break;
            }
        }

        public virtual void SetHealth(float max)
        {
            MaxHealth = max;
            CurrentHealth = max;
        }

        public virtual void Damage(float damage, Player attacker = null, params string[] tags)
        {
            if (dead || MaxHealth < 0f || !ProcessTags(attacker, tags))
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

        public virtual bool ProcessTags(Player attacker = null, params string[] tags)
        {
            if (tags == null)
                return true;

            foreach (string tag in tags)
            {
                switch (tag)
                {
                    case ConstStrings.InstakillBreakablesTag:
                        Destroy();
                        return false;
                    case ConstStrings.MoveGridBreakablesTag:
                        if (attacker != null && Mover.ReferenceHub.PlayerId == attacker.PlayerId)
                            Move(!IsMoving, attacker, SnappingModes.Grid);
                        return false;
                    case ConstStrings.MoveNoneBreakablesTag:
                        if (attacker != null && Mover.ReferenceHub.PlayerId == attacker.PlayerId)
                            Move(!IsMoving, attacker, SnappingModes.None);
                        return false;
                    case ConstStrings.MoveNoRotBreakablesTag:
                        if (attacker != null && Mover.ReferenceHub.PlayerId == attacker.PlayerId)
                            Move(!IsMoving, attacker, SnappingModes.NoRot);
                        return false;
                }
            }

            return true;
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

        public void Move(bool state, Player mover = null, SnappingModes mode = SnappingModes.DontMove)
        {
            if (!state || mover == null || mode == SnappingModes.DontMove)
            {
                Mover = null;
                MoveMode = SnappingModes.DontMove;
                return;
            }

            Mover = mover;
            MoveMode = mode;
        }
    }
}
