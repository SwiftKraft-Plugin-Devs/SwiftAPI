using PluginAPI.Core;

namespace SwiftAPI.Utility.Targeters
{
    public class FullInventoryTargeter : PlayerAttributeTargeter
    {
        public override bool GetAttribute(Player p) => p.IsInventoryFull;

        public override string GetTargeterName() => "INVFULL";

        public override string GetTargeterDescription() => "All human players with full inventory.";
    }
}
