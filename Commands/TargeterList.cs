using CommandSystem;
using SwiftAPI.Utility.Targeters;

namespace SwiftAPI.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class TargeterList : CommandBase
    {
        public override string[] GetAliases() => new string[] { "tlist" };

        public override string GetCommandName() => "targeterlist";

        public override string GetDescription() => "Lists out all the targeters available. ";

        public override PlayerPermissions[] GetPerms() => null;

        public override bool Function(string[] args, ICommandSender sender, out string result)
        {
            result = "Registered Targeters: \n\n";

            foreach (TargeterBase targ in TargeterManager.RegisteredTargeters.Values)
                result += "* @" + targ.GetTargeterName() + " - " + targ.GetTargeterDescription() + "\n";

            return true;
        }
    }
}
