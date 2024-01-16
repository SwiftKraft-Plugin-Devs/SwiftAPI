using PluginAPI.Core;

namespace SwiftAPI.Utility.Targeters
{
    public class MTFTargeter : HumanTargeter
    {
        public override bool GetAttribute(Player p) => base.GetAttribute(p) && p.IsNTF;

        public override string GetTargeterName() => "MTF";

        public override string GetTargeterDescription() => "All human players that are MTF Operatives.";
    }
}
