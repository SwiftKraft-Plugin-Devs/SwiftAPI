using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;
using PluginAPI.Helpers;
using SwiftAPI.API.CustomItems;
using SwiftAPI.HarmonyPatches;
using SwiftAPI.Utility.Spawners;
using SwiftAPI.Utility.Targeters;
using System.IO;

namespace SwiftAPI
{
    public class Plugin
    {
        public static readonly string PluginFolder = Path.Combine(Paths.LocalPlugins.Plugins, Name);

        private const string Author = "SwiftKraft";

        private const string Name = "SwiftAPI";

        private const string Description = "A library plugin for easier development of SCP: SL NWAPI plugins.";

        private const string Version = "v1.2";

        [PluginPriority(LoadPriority.Highest)]
        [PluginEntryPoint(Name, Version, Description, Author)]
        public void Init()
        {
            HarmonyPatcher.InitHarmony();

            Log.Info("SwiftAPI Loaded! Version: " + Version);

            EventManager.RegisterEvents<EventHandler>(this);

            new AliveTargeter().Register();
            new AllTargeter().Register();
            new ArmedTargeter().Register();
            new CITargeter().Register();
            new DClassTargeter().Register();
            new DeadTargeter().Register();
            new EmptyInventoryTargeter().Register();
            new FullInventoryTargeter().Register();
            new HumanTargeter().Register();
            new MTFTargeter().Register();
            new ScientistTargeter().Register();
            new SCPTargeter().Register();
            new UnarmedTargeter().Register();

            StaticUnityMethods.OnFixedUpdate += SpawnerManager.FixedUpdate;

            SpawnerManager.RegisterSpawnerType<ItemSpawner>("item");
            SpawnerManager.RegisterSpawnerType<PlayerSpawner>("role");

            CustomItemManager.RegisterItem("API.DECON", new CustomItemFirearm()
            {
                BaseItem = ItemType.GunCOM18,
                DisplayName = "SwiftAPI Breakable Deconstructor",
                Description = "Instantly destroys breakable toys regardless of health.",
                HipData = new CustomFirearmData()
                {
                    Spread = 0f,
                    AirSpread = 0f,
                    RunSpread = 0f,
                    HeadDamage = 99999f,
                    BodyDamage = 99999f,
                    LimbDamage = 99999f,
                    SCPDamage = 99999f,
                    MagazineSize = 230
                },
                InfiniteAmmo = true,
                Tags = [ConstStrings.InstakillBreakablesTag, ConstStrings.APIWeaponTag]
            });

            CustomItemManager.RegisterItem("API.MOVER.GRID", new CustomItemFirearm()
            {
                BaseItem = ItemType.GunCOM18,
                DisplayName = "SwiftAPI Breakable Mover (Grid)",
                Description = "Moves breakables.",
                HipData = new CustomFirearmData()
                {
                    Spread = 0f,
                    AirSpread = 0f,
                    RunSpread = 0f,
                    HeadDamage = 0f,
                    BodyDamage = 0f,
                    LimbDamage = 0f,
                    SCPDamage = 0f,
                    MagazineSize = 230
                },
                InfiniteAmmo = true,
                Tags = [ConstStrings.MoveGridBreakablesTag, ConstStrings.APIWeaponTag]
            });

            CustomItemManager.RegisterItem("API.MOVER.NONE", new CustomItemFirearm()
            {
                BaseItem = ItemType.GunCOM18,
                DisplayName = "SwiftAPI Breakable Mover (None)",
                Description = "Moves breakables.",
                HipData = new CustomFirearmData()
                {
                    Spread = 0f,
                    AirSpread = 0f,
                    RunSpread = 0f,
                    HeadDamage = 0f,
                    BodyDamage = 0f,
                    LimbDamage = 0f,
                    SCPDamage = 0f,
                    MagazineSize = 230
                },
                InfiniteAmmo = true,
                Tags = [ConstStrings.MoveNoneBreakablesTag, ConstStrings.APIWeaponTag]
            });

            CustomItemManager.RegisterItem("API.MOVER.NOROT", new CustomItemFirearm()
            {
                BaseItem = ItemType.GunCOM18,
                DisplayName = "SwiftAPI Breakable Mover (NoRot)",
                Description = "Moves breakables.",
                HipData = new CustomFirearmData()
                {
                    Spread = 0f,
                    AirSpread = 0f,
                    RunSpread = 0f,
                    HeadDamage = 0f,
                    BodyDamage = 0f,
                    LimbDamage = 0f,
                    SCPDamage = 0f,
                    MagazineSize = 230
                },
                InfiniteAmmo = true,
                Tags = [ConstStrings.MoveNoRotBreakablesTag, ConstStrings.APIWeaponTag]
            });
        }

        [PluginUnload]
        public void Unload()
        {
            HarmonyPatcher.DeinitHarmony();
        }
    }
}
