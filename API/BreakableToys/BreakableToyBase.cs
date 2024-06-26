﻿using AdminToys;
using Mirror;
using PlayerRoles;
using PlayerStatsSystem;
using PluginAPI.Core.Items;
using SwiftAPI.API.CustomItems;
using SwiftAPI.Utility.Misc;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SwiftAPI.API.BreakableToys
{
    public class BreakableToyBase : MonoBehaviour
    {
        public PrimitiveObjectToy Toy;

        public BreakableToyHitbox Hitbox;

        public float MaxHealth;

        protected float CurrentHealth;

        public ItemType DropItem = ItemType.None;
        public CustomItemBase DropCustomItem;

        public Faction Faction { get; set; } = Faction.Unclassified;

        public event Action OnBreak;

        public uint NetworkId => Toy.netId;

        public bool IsMoving => Mover != null && MoveMode != SnappingModes.DontMove;

        bool dead;

        float moveOffset;

        SnappingModes MoveMode = SnappingModes.DontMove;

        ReferenceHub Mover;

        protected virtual void Awake()
        {
            Collider col = GetComponentInChildren<Collider>();
            BreakableToyHitbox hitbox = col.gameObject.AddComponent<BreakableToyHitbox>();
            hitbox.Parent = this;
            hitbox.Collider = col;
            Hitbox = hitbox;
        }

        protected virtual void Start()
        {
            Toy.IsStatic = false;
        }

        protected virtual void Update()
        {
            if (!IsMoving)
                return;

            Vector3 targetPos = Mover.PlayerCameraReference.position + (Mover.PlayerCameraReference.forward * moveOffset);

            switch (MoveMode)
            {
                case SnappingModes.None:
                    Toy.transform.SetPositionAndRotation(targetPos, Mover.PlayerCameraReference.rotation);
                    break;
                case SnappingModes.Grid:
                    Toy.transform.position = GridSnapper.SnapToGrid(targetPos, Toy.NetworkScale, Vector3.one / 4f);
                    break;
                case SnappingModes.NoRot:
                    Toy.transform.SetPositionAndRotation(targetPos, Quaternion.identity);
                    break;
            }
        }

        public virtual void SetHealth(float max)
        {
            MaxHealth = max;
            CurrentHealth = max;
        }

        public virtual void Damage(float damage, ReferenceHub attacker = null, params string[] tags)
        {
            if (dead || !ProcessTags(attacker, tags) || MaxHealth < 0f)
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

            OnBreak?.Invoke();

            BreakableToyManager.Breakables.Remove(this);

            NetworkServer.Destroy(Toy.gameObject);
        }

        public virtual bool ProcessTags(ReferenceHub attacker = null, params string[] tags)
        {
            if (tags == null || attacker == null)
                return true;

            foreach (string tag in tags)
            {
                switch (tag)
                {
                    case ConstStrings.InstakillBreakablesTag:
                        Destroy();
                        return false;
                    case ConstStrings.MoveGridBreakablesTag:
                        Move(!IsMoving, attacker, SnappingModes.Grid);
                        return false;
                    case ConstStrings.MoveNoneBreakablesTag:
                        Move(!IsMoving, attacker, SnappingModes.None);
                        return false;
                    case ConstStrings.MoveNoRotBreakablesTag:
                        Move(!IsMoving, attacker, SnappingModes.NoRot);
                        return false;
                }
            }

            return true;
        }

        public virtual void Drop()
        {
            if (DropCustomItem == null && DropItem != ItemType.None)
            {
                ItemPickup ib = ItemPickup.Create(DropItem, Toy.NetworkPosition, Quaternion.identity);
                ib.Spawn();
            }
            else
                CustomItemManager.DropCustomItem(DropCustomItem, Toy.NetworkPosition);
        }

        public virtual void Move(bool state, ReferenceHub mover = null, SnappingModes mode = SnappingModes.DontMove)
        {
            if (!state || mover == null || mode == SnappingModes.DontMove)
            {
                Mover = null;
                MoveMode = SnappingModes.DontMove;
                Toy.IsStatic = true;
                moveOffset = 0f;
                return;
            }

            Toy.IsStatic = false;
            Mover = mover;
            MoveMode = mode;
            moveOffset = Vector3.Distance(Mover.PlayerCameraReference.position, transform.position);
        }
    }
}
