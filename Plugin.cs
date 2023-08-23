using CustomItemAPI.API;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;

namespace CustomItemAPI
{
    public class Plugin
    {
        private const string Author = "KPG Dev Team";

        private const string Name = "Custom Items API";

        private const string Description = "Allows for other plugins to create custom items. ";

        private const string Version = "0.0.1";

        /// <summary>
        /// Set this to true when you want to spam logs and stuff. 
        /// Make sure to add a check for this to the spammy logs.
        /// For development only.
        /// </summary>
        public static bool DebugMode = true;

        [PluginPriority(LoadPriority.Lowest)]
        [PluginEntryPoint(Name, Version, Description, Author)]
        public void Init()
        {
            Log.Info("Custom Items API Loaded! ");

            EventManager.RegisterEvents<EventHandler>(this);

            if (DebugMode)
            {
                CustomItemManager.RegisterItem("debug_item", new CustomItemEquippable() { BaseItem = ItemType.KeycardJanitor, DisplayName = "Debug Item" });
                CustomItemManager.RegisterItem("debug_gun", new CustomItemFirearm() 
                { 
                    BaseItem = ItemType.GunCOM15,
                    DisplayName = "Debug Gun",
                    Data = new CustomFirearmData() 
                    {
                        HeadDamage = 100f,
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
