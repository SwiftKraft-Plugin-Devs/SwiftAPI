using PlayerRoles;
using PluginAPI.Core;

namespace SwiftAPI.Utility.Targeters
{
    public class SCPTargeter : PlayerAttributeTargeter
    {
        public override bool GetAttribute(Player p) => p.IsSCP && p.Role != RoleTypeId.Flamingo && p.Role != RoleTypeId.AlphaFlamingo;

        public override string GetTargeterName() => "SCP";
    }
}
