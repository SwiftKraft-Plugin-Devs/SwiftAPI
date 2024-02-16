using CommandSystem;
using SwiftAPI.API.ServerVariables;

namespace SwiftAPI.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SetServerVariable : CommandBase
    {
        public override string[] GetAliases() => ["svar", "var"];

        public override string GetCommandName() => "servervar";

        public override string GetDescription() => "Sets a server variable.";

        public override PlayerPermissions[] GetPerms() => [PlayerPermissions.RoundEvents];

        public override bool Function(string[] args, ICommandSender sender, out string result)
        {
            if (!TryGetArgument(args, 1, out string arg1))
            {
                result = "Please input a server variable ID! ";

                return false;
            }

            if (!TryGetArgument(args, 2, out string arg2))
            {
                result = "Please input a variable to be parsed! ";

                return false;
            }

            ServerVariableManager.SetVar(arg1, arg2);

            result = "Set variable " + arg1 + " to " + arg2;

            return true;
        }
    }
}
