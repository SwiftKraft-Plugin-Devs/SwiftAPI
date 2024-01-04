using PluginAPI.Core;

namespace CustomItemAPI.Utility
{
    public class CITargeter : PlayerAttributeTargeter
    {
        public override bool GetAttribute(Player p) => p.IsChaos;

        public override string GetTargeterName() => "CI";
    }
}
