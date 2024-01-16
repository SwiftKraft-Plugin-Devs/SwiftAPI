using UnityEngine;

namespace SwiftAPI.Utility.Spawners
{
    public abstract class SpawnerBase
    {
        public Vector3 Position;
        public float MaxTimer;

        public int ID => SpawnerManager.Spawners.IndexOf(this);

        public bool Active = true;

        protected float CurrentTimer;

        public SpawnerBase Initialize(Vector3 pos, float timer)
        {
            Position = pos;
            MaxTimer = timer;

            Active = true;

            Init();

            return this;
        }

        public virtual void Init()
        {
            CurrentTimer = MaxTimer;
        }

        /// <summary>
        /// For commands.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract bool SetSpawnee(string value, out string feedback);

        public virtual void FixedUpdate()
        {
            if (!Active)
                return;

            if (CurrentTimer > 0f)
                CurrentTimer -= Time.fixedDeltaTime;
            else
            {
                CurrentTimer = MaxTimer;
                Spawn();
            }
        }

        public abstract void Spawn();

        public override string ToString() => "\n// Spawner Data // ===========\nSpawner Type: " + GetType().ToString() + "\nID: " + ID + "\nTimer: " + MaxTimer + "\nPosition: " + Position + "\nActive: " + Active + "\n// Type Specifics // ===========";
    }
}
