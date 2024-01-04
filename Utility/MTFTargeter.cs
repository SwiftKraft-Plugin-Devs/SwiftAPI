using PluginAPI.Core;

namespace SwiftAPI.Utility
{
    public class MTFTargeter : PlayerAttributeTargeter
    {
        public override bool GetAttribute(Player p) => p.IsNTF;

        public override string GetTargeterName() => "MTF";
    }
}
