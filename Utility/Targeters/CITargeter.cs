using PluginAPI.Core;

namespace SwiftAPI.Utility.Targeters
{
    public class CITargeter : HumanTargeter
    {
        public override bool GetAttribute(Player p) => base.GetAttribute(p) && p.IsChaos;

        public override string GetTargeterName() => "CI";
    }
}
