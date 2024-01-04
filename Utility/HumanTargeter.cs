using PluginAPI.Core;

namespace CustomItemAPI.Utility
{
    public class HumanTargeter : PlayerAttributeTargeter
    {
        public override bool GetAttribute(Player p) => p.IsHuman;

        public override string GetTargeterName() => "HUMAN";
    }
}
