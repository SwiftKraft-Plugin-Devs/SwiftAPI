using CommandSystem;
using SwiftAPI.API.ServerVariables;

namespace SwiftAPI.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ListServerVariables : CommandBase
    {
        public override string[] GetAliases() => new string[] { "lsvar" };

        public override string GetCommandName() => "listservervars";

        public override string GetDescription() => "Lists all server variables. ";

        public override PlayerPermissions[] GetPerms() => null;

        public override bool Function(string[] args, ICommandSender sender, out string result)
        {
            result = "Server Variables: ";

            foreach (string key in ServerVariableManager.Vars.Keys)
                result += "\n " + key + " - " + ServerVariableManager.Vars[key];

            return true;
        }
    }
}
