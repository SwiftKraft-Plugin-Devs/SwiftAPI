using CommandSystem;
using SwiftAPI.API.BreakableToys;

namespace SwiftAPI.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ClearBreakables : CommandBase
    {
        public override string[] GetAliases() => ["clrbrks"];

        public override string GetCommandName() => "clearbreakables";

        public override string GetDescription() => "Clears all breakables.";

        public override PlayerPermissions[] GetPerms() => [PlayerPermissions.RoundEvents];

        public override bool Function(string[] args, ICommandSender sender, out string result)
        {
            BreakableToyManager.ClearBreakables();

            result = "Cleared all breakables! ";

            return true;
        }
    }
}
