using CommandSystem;
using SwiftAPI.Utility.Spawners;

namespace SwiftAPI.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ListSpawnerTypes : CommandBase
    {
        public override string[] GetAliases() => new string[] { "lspt" };

        public override string GetCommandName() => "listsptypes";

        public override string GetDescription() => "Lists out all the registered spawner types. ";

        public override PlayerPermissions[] GetPerms() => null;

        public override bool Function(string[] args, ICommandSender sender, out string result)
        {
            result = "All Spawner Types: ";

            foreach (string s in SpawnerManager.SpawnerTypes.Keys)
                result += "\n" + s + " - " + SpawnerManager.SpawnerTypes[s];

            return true;
        }
    }
}
