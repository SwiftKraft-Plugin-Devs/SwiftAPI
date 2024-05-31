using CommandSystem;
using SwiftAPI.Utility.Spawners;
using System.Collections.Generic;

namespace SwiftAPI.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class EditSpawner : CommandBase
    {
        public override string[] GetAliases() => ["esp"];

        public override string GetCommandName() => "editsp";

        public override string GetDescription() => "Edits the custom data of a spawner.";

        public override PlayerPermissions[] GetPerms() => [PlayerPermissions.GivingItems];

        public override bool Function(string[] args, ICommandSender sender, out string result)
        {
            if (!TryGetArgument(args, 1, out string arg1) || !int.TryParse(arg1, out int id) || !SpawnerManager.IsValidSpawner(id))
            {
                result = "Cannot find spawner!";

                return false;
            }

            if (args.Length < 3)
            {
                result = "Please input the new custom data for spawner ID " + id + "!";

                return false;
            }

            List<string> customData = [.. args];
            customData.RemoveRange(0, 2);

            return SpawnerManager.Spawners[id].SetSpawnee([.. customData], out result);
        }
    }
}
