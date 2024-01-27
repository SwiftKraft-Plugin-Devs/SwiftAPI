using PlayerRoles;
using PluginAPI.Core;

namespace SwiftAPI.Utility.Targeters
{
    public class SCPTargeter : PlayerAttributeTargeter
    {
        public override bool GetAttribute(Player p) => p.IsSCP;

        public override string GetTargeterName() => "SCP";

        public override string GetTargeterDescription() => "All SCP players.";
    }
}
