using CommandSystem;
using SwiftAPI.Utility.Spawners;

namespace SwiftAPI.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class EditSpawner : CommandBase
    {
        public override string[] GetAliases() => new string[] { "esp" };

        public override string GetCommandName() => "editsp";

        public override string GetDescription() => "Edits the custom data of a spawner.";

        public override PlayerPermissions[] GetPerms() => new PlayerPermissions[] { PlayerPermissions.GivingItems };

        public override bool Function(string[] args, ICommandSender sender, out string result)
        {
            if (!TryGetArgument(args, 1, out string arg1) || !int.TryParse(arg1, out int id) || !SpawnerManager.IsValidSpawner(id))
            {
                result = "Cannot find spawner!";

                return false;
            }

            if (!TryGetArgument(args, 2, out string arg2))
            {
                result = "Please input the new custom data for spawner ID " + id + "!";

                return false;
            }

            return SpawnerManager.Spawners[id].SetSpawnee(arg2, out result);
        }
    }
}
