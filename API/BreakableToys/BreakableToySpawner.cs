using SwiftAPI.Utility.Spawners;
using UnityEngine;

namespace SwiftAPI.API.BreakableToys
{
    public class BreakableToySpawner : BreakableToyBase
    {
        public SpawnerBase Spawner { get; private set; }
        public Vector3 SpawnerOffset { get; private set; }

        protected override void Update()
        {
            base.Update();

            if (!IsMoving)
                return;

            UpdateSpawnerPosition();
        }

        public override void Destroy()
        {
            base.Destroy();

            if (Spawner != null)
                SpawnerManager.RemoveSpawner(Spawner);
        }

        public void UpdateSpawnerPosition() 
        { 
            if (Spawner != null) 
                Spawner.Position = transform.position + SpawnerOffset; 
        }

        public void Setup(SpawnerBase spawner, Vector3 spawnerOffset)
        {
            Spawner = spawner;
            SpawnerOffset = spawnerOffset;
            UpdateSpawnerPosition();
        }

        public void Setup(SpawnerBase spawner)
        {
            Spawner = spawner;
            UpdateSpawnerPosition();
        }

        public void Setup(Vector3 spawnerOffset)
        {
            SpawnerOffset = spawnerOffset;
            UpdateSpawnerPosition();
        }
    }
}
