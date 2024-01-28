using CommandSystem;
using SwiftAPI.API.BreakableToys;

namespace SwiftAPI.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ClearBreakables : CommandBase
    {
        public override string[] GetAliases() => new string[] { "clrbrks" };

        public override string GetCommandName() => "clearbreakables";

        public override string GetDescription() => "Clears all breakables.";

        public override PlayerPermissions[] GetPerms() => new PlayerPermissions[] { PlayerPermissions.RoundEvents };

        public override bool Function(string[] args, ICommandSender sender, out string result)
        {
            BreakableToyManager.ClearBreakables();

            result = "Cleared all breakables! ";

            return true;
        }
    }
}
