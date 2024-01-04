using PluginAPI.Core;

namespace CustomItemAPI.Utility
{
    public class FullInventoryTargeter : PlayerAttributeTargeter
    {
        public override bool GetAttribute(Player p) => p.IsInventoryFull;

        public override string GetTargeterName() => "INVFULL";
    }
}
