using PluginAPI.Core;

namespace SwiftAPI.Utility.Targeters
{
    public class HumanTargeter : PlayerAttributeTargeter
    {
        public override bool GetAttribute(Player p) => p.IsHuman;

        public override string GetTargeterName() => "HUMAN";
    }
}
