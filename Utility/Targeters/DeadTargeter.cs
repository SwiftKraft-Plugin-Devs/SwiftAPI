using PlayerRoles;
using PluginAPI.Core;

namespace SwiftAPI.Utility.Targeters
{
    public class DeadTargeter : PlayerAttributeTargeter
    {
        public override bool GetAttribute(Player p) => p.Role == RoleTypeId.Spectator;

        public override string GetTargeterName() => "DEAD";

        public override string GetTargeterDescription() => "All players that are currently in spectator.";
    }
}
