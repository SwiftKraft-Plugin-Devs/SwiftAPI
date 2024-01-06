using SwiftAPI.API.CustomItems;
using SwiftAPI.API.CustomItems.FriendlyActions;
using SwiftAPI.Utility.Targeters;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;

namespace SwiftAPI
{
    public class Plugin
    {
        private const string Author = "SwiftKraft";

        private const string Name = "SwiftAPI";

        private const string Description = "A library plugin for easier development of SCP: SL NWAPI plugins.";

        private const string Version = "v0.0.1";

        /// <summary>
        /// Set this to true when you want to spam logs and stuff.
        /// Make sure to add a check for this to the spammy logs.
        /// For development only.
        /// </summary>
        public static bool DebugMode = false;

        [PluginPriority(LoadPriority.Lowest)]
        [PluginEntryPoint(Name, Version, Description, Author)]
        public void Init()
        {
            Log.Info("SwiftAPI Loaded! Version: " + Version);

            EventManager.RegisterEvents<EventHandler>(this);

            new AllTargeter();
            new AliveTargeter();
            new ArmedTargeter();
            new CITargeter();
            new DClassTargeter();
            new FlamingoTargeter();
            new FullInventoryTargeter();
            new HumanTargeter();
            new MTFTargeter();
            new ScientistTargeter();
            new SCPTargeter();
            new UnarmedTargeter();

            if (DebugMode)
            {
                CustomItemManager.RegisterItem("debug_item", new CustomItemEquippable() { BaseItem = ItemType.KeycardJanitor, DisplayName = "Debug Item" });
                CustomItemManager.RegisterItem("debug_grenade", new CustomItemTimeGrenade()
                {
                    BaseItem = ItemType.GrenadeHE,
                    DisplayName = "Debug Grenade",
                    Data = new CustomGrenadeData()
                    {
                        FuseTime = 0.1f
                    }
                });
                CustomItemManager.RegisterItem("debug_flashbang", new CustomItemTimeGrenade()
                {
                    BaseItem = ItemType.GrenadeFlash,
                    DisplayName = "Debug Flash",
                    Data = new CustomGrenadeData()
                    {
                        FuseTime = 0.1f
                    }
                });
                CustomItemManager.RegisterItem("debug_gun", new CustomItemFirearm()
                {
                    BaseItem = ItemType.GunCOM18,
                    DisplayName = "Debug Gun",
                    HipData = new CustomFirearmData()
                    {
                        HeadDamage = 100f,
                        BodyDamage = 25f,
                        LimbDamage = 20f,
                        SCPDamage = 25f,
                        MagazineSize = 99,
                        FriendlyAction = new FriendlyActionHeal()
                        {
                            Amount = 30f
                        }
                    },
                    AimData = new CustomFirearmData()
                    {
                        HeadDamage = 100f,
                        BodyDamage = 25f,
                        LimbDamage = 20f,
                        SCPDamage = 25f,
                        MagazineSize = 255,
                        FriendlyAction = new FriendlyActionSpeed()
                        {
                            Intensity = 30,
                            Duration = 3f
                        }
                    }
                });
                CustomItemManager.RegisterItem("debug_gun2", new CustomItemFirearm()
                {
                    BaseItem = ItemType.GunShotgun,
                    DisplayName = "Debug Gun 2",
                    HipData = new CustomFirearmData()
                    {
                        HeadDamage = 100f,
                        BodyDamage = 25f,
                        LimbDamage = 20f,
                        SCPDamage = 25f
                    },
                    AimData = new CustomFirearmData()
                    {
                        HeadDamage = 100f,
                        BodyDamage = 25f,
                        LimbDamage = 20f,
                        SCPDamage = 25f
                    },
                });
                CustomItemManager.RegisterItem("debug_gun3", new CustomItemFirearm()
                {
                    BaseItem = ItemType.GunRevolver,
                    DisplayName = "Debug Gun 3",
                    HipData = new CustomFirearmData()
                    {
                        HeadDamage = 200f,
                        BodyDamage = 25f,
                        LimbDamage = 20f,
                        SCPDamage = 25f,
                        MagazineSize = 99,
                        FriendlyAction = new FriendlyActionShield()
                        {
                            Amount = 5,
                            Limit = 10,
                            Sustain = 3,
                            Efficacy = 100,
                            Decay = 5
                        }
                    },
                });
                CustomItemManager.RegisterItem("debug_gun4", new CustomItemFirearm()
                {
                    BaseItem = ItemType.ParticleDisruptor,
                    DisplayName = "Debug Gun 4",
                    HipData = new CustomFirearmData()
                    {
                        HeadDamage = 200f,
                        BodyDamage = 25f,
                        LimbDamage = 20f,
                        SCPDamage = 25f,
                        MagazineSize = 99
                    }
                });
            }
        }
    }
}
