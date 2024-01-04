using CommandSystem;
using CustomItemAPI.API;

namespace CustomItemAPI.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class CustomList : CommandBase
    {
        public override string[] GetAliases()
        {
            return new string[] { "clist" };
        }

        public override string GetCommandName()
        {
            return "customitemlist";
        }

        public override string GetDescription()
        {
            return "Lists all registered custom items.";
        }

        public override PlayerPermissions[] GetPerms()
        {
            return new PlayerPermissions[] { };
        }

        public override bool Function(string[] args, ICommandSender sender, out string result)
        {
            result = "Registered Custom Items: \n\n";

            foreach (CustomItemBase cust in CustomItemManager.RegisteredItems.Values)
                result += "* " + cust.CustomItemID + " - " + cust.DisplayName + "\n";

            return true;
        }
    }
}
