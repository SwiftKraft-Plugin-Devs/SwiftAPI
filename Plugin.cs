using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;

namespace CustomItemAPI
{
    public class Plugin
    {
        private const string Author = "KPG Dev Team";

        private const string Name = "Custom Items API";

        private const string Description = "Allows for other plugins to create custom items. ";

        public const string Version = "0.0.1";

        [PluginPriority(LoadPriority.Lowest)]
        [PluginEntryPoint(Name, Version, Description, Author)]
        public void Init()
        {
            Log.Info("Custom Items API Loaded! ");
        }
    }
}
