using PluginAPI.Core;

namespace CustomItemAPI.Utility
{
    public class FlamingoTargeter : PlayerAttributeTargeter
    {
        public override bool GetAttribute(Player p) => p.Role == PlayerRoles.RoleTypeId.Flamingo || p.Role == PlayerRoles.RoleTypeId.AlphaFlamingo;

        public override string GetTargeterName() => "FLA";
    }
}
