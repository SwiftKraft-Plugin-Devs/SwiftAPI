using CommandSystem;
using SwiftAPI.Utility.Spawners;

namespace SwiftAPI.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ToggleSpawner : CommandBase
    {
        public override string[] GetAliases() => ["tsp"];

        public override string GetCommandName() => "togglesp";

        public override string GetDescription() => "Disables or enables spawners. ";

        public override PlayerPermissions[] GetPerms() => [PlayerPermissions.GivingItems];

        public override bool Function(string[] args, ICommandSender sender, out string result)
        {
            if (!TryGetArgument(args, 1, out string arg1) || !int.TryParse(arg1, out int id))
            {
                result = "Please input an integer!";

                return false;
            }

            if (!TryGetArgument(args, 2, out string arg2) || !bool.TryParse(arg2, out bool status))
            {
                if (!SpawnerManager.ToggleSpawner(id))
                {
                    result = "Out of range! ";

                    return false;
                }
                else
                {
                    result = $"Spawner {id}: " + SpawnerManager.Spawners[id].ToString();

                    return true;
                }
            }

            if (!SpawnerManager.ToggleSpawner(id, status))
            {
                result = "Out of range! ";

                return false;
            }
            else
            {
                result = $"Spawner {id}: " + SpawnerManager.Spawners[id].ToString();

                return true;
            }
        }
    }
}
