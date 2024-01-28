using AdminToys;
using Footprinting;
using Mirror;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SwiftAPI.API.BreakableToys
{
    public static class BreakableToyManager
    {
        public static PrimitiveObjectToy Prefab;

        public static readonly List<BreakableToyBase> Breakables = new List<BreakableToyBase>();

        private static void RegisterPrefab()
        {
            foreach (GameObject value in NetworkClient.prefabs.Values)
                if (value.TryGetComponent(out PrimitiveObjectToy component))
                    Prefab = component;
        }

        public static T SpawnBreakableToy<T>(this ReferenceHub admin, PrimitiveType type, Vector3 position, Quaternion rotation, Vector3 size, Color color) where T : BreakableToyBase
        {
            if (Prefab == null)
                RegisterPrefab();

            PrimitiveObjectToy spawnee = Object.Instantiate(Prefab);
            spawnee.NetworkPrimitiveType = type;
            spawnee.transform.SetPositionAndRotation(position, rotation);
            spawnee.transform.localScale = size;
            spawnee.NetworkScale = size;
            spawnee.NetworkMaterialColor = color;
            spawnee.SpawnerFootprint = new Footprint(admin);
            T b = spawnee.gameObject.AddComponent<T>();
            b.Toy = spawnee;
            Breakables.Add(b);
            NetworkServer.Spawn(spawnee.gameObject);

            return b;
        }

        public static void ClearBreakables()
        {
            foreach (BreakableToyBase b in Breakables)
                b.Destroy();
        }
    }
}
