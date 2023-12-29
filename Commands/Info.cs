using CommandSystem;
using CustomItemAPI.API;
using PluginAPI.Core;

namespace CustomItemAPI.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Info : CommandBase
    {
        public override string[] GetAliases()
        {
            return new string[] { "inf", "whatdo", "check", "helpitem" };
        }

        public override string GetCommandName()
        {
            return "info";
        }

        public override string GetDescription()
        {
            return "Tells you what the item you are holding does. ";
        }

        public override bool GetRequirePlayer() => true;

        public override PlayerPermissions[] GetPerms()
        {
            return new PlayerPermissions[] { };
        }

        public override bool PlayerBasedFunction(Player player, string[] args, out string result)
        {
            Log.Info("Showing info command!");

            if (player.CurrentItem == null)
            {
                result = "Please hold the item you are trying to look up info of! ";

                return false;
            }

            if (CustomItemManager.TryGetCustomItemWithSerial(player.CurrentItem.ItemSerial, out CustomItemBase cust))
            {
                result = $"\nItem \"{cust.DisplayName}\": \nInternal ID: \"{cust.CustomItemID}\"\nDescription: \n{cust.Description}";

                return true;
            }
            else
            {
                result = "The item you are holding is NOT a custom item.";

                return false;
            }
        }
    }
}
