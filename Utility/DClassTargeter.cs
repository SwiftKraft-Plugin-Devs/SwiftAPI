using PluginAPI.Core;

namespace CustomItemAPI.Utility
{
    public class DClassTargeter : PlayerAttributeTargeter
    {
        public override bool GetAttribute(Player p) => p.Role == PlayerRoles.RoleTypeId.ClassD;

        public override string GetTargeterName() => "CD";
    }
}
