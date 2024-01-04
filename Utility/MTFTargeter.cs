using PluginAPI.Core;

namespace CustomItemAPI.Utility
{
    public class MTFTargeter : PlayerAttributeTargeter
    {
        public override bool GetAttribute(Player p) => p.IsNTF;

        public override string GetTargeterName() => "MTF";
    }
}
