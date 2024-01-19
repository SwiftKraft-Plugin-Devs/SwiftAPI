using CommandSystem;
using SwiftAPI.Utility.Spawners;

namespace SwiftAPI.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ToggleAllSpawners : CommandBase
    {
        public override string[] GetAliases() => new string[] { "tasp" };

        public override string GetCommandName() => "toggleallsp";

        public override string GetDescription() => "Pauses or unpauses all spawners.";

        public override PlayerPermissions[] GetPerms() => new PlayerPermissions[] { PlayerPermissions.GivingItems };

        public override bool Function(string[] args, ICommandSender sender, out string result)
        {
            if (!TryGetArgument(args, 1, out string arg1) || !bool.TryParse(arg1, out bool value))
            {
                result = "Please input a boolean! (true/false)";

                return false;
            }

            foreach (SpawnerBase sp in SpawnerManager.Spawners)
                sp.Active = value;

            result = "Toggled all spawners to " + value + "! ";

            return true;
        }
    }
}
