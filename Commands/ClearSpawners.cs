using CommandSystem;
using SwiftAPI.Utility.Spawners;

namespace SwiftAPI.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ClearSpawners : CommandBase
    {
        public override string[] GetAliases() => new string[] { "csp" };

        public override string GetCommandName() => "clearsp";

        public override string GetDescription() => "Removes all spawners. ";

        public override PlayerPermissions[] GetPerms() => new PlayerPermissions[] { PlayerPermissions.GivingItems };

        public override bool Function(string[] args, ICommandSender sender, out string result)
        {
            result = "Cleared all spawners! ";

            SpawnerManager.ClearSpawners();

            return true;
        }
    }
}
