using CommandSystem;
using InventorySystem.Items;
using PluginAPI.Core;
using SwiftAPI.API.CustomItems;

namespace SwiftAPI.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Info : CommandBase
    {
        public override string[] GetAliases() => ["inf", "whatdo", "check", "helpitem"];

        public override string GetCommandName() => "info";

        public override string GetDescription() => "Tells you what the item you are holding does. ";

        public override bool GetRequirePlayer() => true;

        public override PlayerPermissions[] GetPerms() => null;

        public override bool PlayerBasedFunction(Player player, string[] args, out string result)
        {
            ItemBase item = player.CurrentItem;

            if (item == null)
            {
                foreach (ushort it in player.ReferenceHub.inventory.UserInventory.Items.Keys)
                    if (CustomItemManager.TryGetCustomItemWithSerial(player.ReferenceHub.inventory.UserInventory.Items[it].ItemSerial, out CustomItemBase invCust))
                    {
                        result = "\n" + invCust.ToString();

                        return true;
                    }

                result = "No custom items detected.";

                return false;
            }

            if (CustomItemManager.TryGetCustomItemWithSerial(item.ItemSerial, out CustomItemBase cust))
            {
                result = "\n" + cust.ToString();

                return true;
            }
            else
            {
                result = "The item you are holding is NOT a custom item.";

                return false;
            }
        }
    }

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class RAInfo : CommandBase
    {
        public override string[] GetAliases() => new string[] { "inf", "whatdo", "check", "helpitem" };

        public override string GetCommandName() => "info";

        public override string GetDescription() => "Tells you what the item you are holding does. ";

        public override PlayerPermissions[] GetPerms() => null;

        public override bool Function(string[] args, ICommandSender sender, out string result)
        {
            if (!TryGetArgument(args, 1, out string arg1) || !CustomItemManager.TryGetCustomItemWithID(arg1, out CustomItemBase cust))
            {
                result = "Please input a valid custom item ID.";

                return false;
            }

            result = cust.ToString();

            return true;
        }
    }
}
