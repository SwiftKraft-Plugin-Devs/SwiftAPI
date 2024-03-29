﻿using CommandSystem;
using SwiftAPI.API.CustomItems;

namespace SwiftAPI.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class CustomList : CommandBase
    {
        public override string[] GetAliases() => ["clist"];

        public override string GetCommandName() => "customitemlist";

        public override string GetDescription() => "Lists all registered custom items.";

        public override PlayerPermissions[] GetPerms() => null;

        public override bool Function(string[] args, ICommandSender sender, out string result)
        {
            result = "Registered Custom Items: \n\n";

            foreach (CustomItemBase cust in CustomItemManager.RegisteredItems.Values)
                result += "* " + cust.CustomItemID + " - " + cust.DisplayName + "\n";

            return true;
        }
    }
}
