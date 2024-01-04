using PluginAPI.Core;

namespace SwiftAPI.Utility
{
    public class FullInventoryTargeter : PlayerAttributeTargeter
    {
        public override bool GetAttribute(Player p) => p.IsInventoryFull;

        public override string GetTargeterName() => "INVFULL";
    }
}
