using PluginAPI.Core;

namespace SwiftAPI.Utility.Targeters
{
    public class DClassTargeter : HumanTargeter
    {
        public override bool GetAttribute(Player p) => base.GetAttribute(p) && p.Role == PlayerRoles.RoleTypeId.ClassD;

        public override string GetTargeterName() => "CD";
    }
}
