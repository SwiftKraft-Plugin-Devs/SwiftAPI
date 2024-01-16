using PluginAPI.Core;

namespace SwiftAPI.Utility.Targeters
{
    public class ScientistTargeter : HumanTargeter
    {
        public override bool GetAttribute(Player p) => base.GetAttribute(p) && p.Role == PlayerRoles.RoleTypeId.Scientist;

        public override string GetTargeterName() => "SCI";

        public override string GetTargeterDescription() => "All human players that are Scientists.";
    }
}
