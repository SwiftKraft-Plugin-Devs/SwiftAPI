using System;
using System.Collections.Generic;
using UnityEngine;

namespace SwiftAPI.Utility.Spawners
{
    public static class SpawnerManager
    {
        public static readonly List<SpawnerBase> Spawners = new();

        public static readonly Dictionary<string, Type> SpawnerTypes = new();

        public static void FixedUpdate()
        {
            foreach (SpawnerBase sp in Spawners)
                if (sp.Active)
                    sp.FixedUpdate();
        }

        public static bool IsValidType(string id) => SpawnerTypes.ContainsKey(id.ToUpper());

        public static bool IsValidSpawner(int id) => Spawners.Count > id;

        public static bool RegisterSpawnerType<T>(string id) where T : SpawnerBase
        {
            if (SpawnerTypes.ContainsKey(id))
                return false;

            SpawnerTypes.Add(id.ToUpper(), typeof(T));
            return true;
        }

        public static bool ToggleSpawner(int id, bool status)
        {
            if (!IsValidSpawner(id))
                return false;

            Spawners[id].Active = status;
            return true;
        }

        public static bool ToggleSpawner(int id)
        {
            if (!IsValidSpawner(id))
                return false;

            return ToggleSpawner(id, !Spawners[id].Active);
        }

        public static bool RemoveSpawner(int spawnerId)
        {
            if (!IsValidSpawner(spawnerId))
                return false;

            Spawners.RemoveAt(spawnerId);
            return true;
        }

        public static void ClearSpawners() => Spawners.Clear();

        public static SpawnerBase AddSpawner(Vector3 pos, float timer, Type type)
        {
            if (type.IsAbstract || type.BaseType != typeof(SpawnerBase))
                return null;

            SpawnerBase instance = (SpawnerBase)Activator.CreateInstance(type);
            Spawners.Add(instance.Initialize(pos, timer));
            return instance;
        }

        /// <summary>
        /// The spawner type has to be registered in order for this to work.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="timer"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SpawnerBase AddSpawner(Vector3 pos, float timer, string id)
        {
            if (SpawnerTypes.ContainsKey(id.ToUpper()))
                return AddSpawner(pos, timer, SpawnerTypes[id.ToUpper()]);
            else
                return null;
        }

        public static T AddSpawner<T>(Vector3 pos, float timer) where T : SpawnerBase
        {
            return (T)AddSpawner(pos, timer, typeof(T));
        }
    }
}
