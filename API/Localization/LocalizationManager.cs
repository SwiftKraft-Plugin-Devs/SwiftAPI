using System.IO;

namespace SwiftAPI.API.Localization
{
    public class LocalizationManager
    {
        public const string FolderName = "Localization";

        public static readonly string LocalizationFolder = Path.Combine(Plugin.PluginFolder, FolderName);
    }
}
