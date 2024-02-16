using CommandSystem;
using SwiftAPI.Utility.Spawners;

namespace SwiftAPI.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class InfoSpawner : CommandBase
    {
        public override string[] GetAliases() => ["isp"];

        public override string GetCommandName() => "infosp";

        public override string GetDescription() => "Shows you the detailed info about a spawner.";

        public override PlayerPermissions[] GetPerms() => null;

        public override bool Function(string[] args, ICommandSender sender, out string result)
        {
            if (!TryGetArgument(args, 1, out string arg1) || !int.TryParse(arg1, out int id))
            {
                result = "Please input a integer! ";

                return false;
            }

            if (!SpawnerManager.IsValidSpawner(id))
            {
                result = "Out of range! ";

                return false;
            }

            result = SpawnerManager.Spawners[id].ToString();

            return true;
        }
    }
}
