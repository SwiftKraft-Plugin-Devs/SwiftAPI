using CommandSystem;
using SwiftAPI.Utility.Spawners;

namespace SwiftAPI.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ListSpawners : CommandBase
    {
        public override string[] GetAliases() => ["lsp"];

        public override string GetCommandName() => "listsp";

        public override string GetDescription() => "Lists all spawners. ";

        public override PlayerPermissions[] GetPerms() => null;

        public override bool Function(string[] args, ICommandSender sender, out string result)
        {
            result = "\nAll Spawners: ";

            foreach (SpawnerBase sp in SpawnerManager.Spawners)
                result += "\n " + sp.ID + " | Position: " + sp.Position + ", Time: " + sp.MaxTimer + $", Active: {(!sp.Active ? "<color=#FF0000>" : "<color=#00FF00>")}" + sp.Active + "</color>";

            return true;
        }
    }
}
