using PluginAPI.Core;

namespace SwiftAPI.Utility
{
    public class ScientistTargeter : PlayerAttributeTargeter
    {
        public override bool GetAttribute(Player p) => p.Role == PlayerRoles.RoleTypeId.Scientist;

        public override string GetTargeterName() => "SCI";
    }
}
