using PluginAPI.Core;

namespace SwiftAPI.Utility
{
    public class SCPTargeter : PlayerAttributeTargeter
    {
        public override bool GetAttribute(Player p) => p.IsSCP;

        public override string GetTargeterName() => "SCP";
    }
}
