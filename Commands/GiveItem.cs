using CommandSystem;
using CustomItemAPI.API;
using PluginAPI.Core;
using System;

namespace CustomItemAPI.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class GiveItem : ICommand
    {
        public string Command { get; } = "givecust";

        public string[] Aliases { get; } = { "gcust", "givec", "getcust", "getc" };

        public string Description { get; } = "Custom Item API command, gives you or a target a registered custom item. Usage: \"givecust <Custom Item ID> [Player ID]\"";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission(PlayerPermissions.GivingItems))
            {
                response = $"No permission! Required: GivingItems";

                return false;
            }

            string itemID;
            string playerOrTargeter = Player.Get(sender).PlayerId.ToString();

            if (arguments.Array.Length < 2)
            {
                response = "Please provide an item ID!";

                return false;
            }

            if (arguments.Array.Length > 2)
                playerOrTargeter = arguments.Array[2];

            itemID = arguments.Array[1];

            if (!CustomItemManager.TryGetCustomItemWithID(itemID, out CustomItemBase cu))
            {
                response = "Item does not exist!";

                return false;
            }

            if ((!int.TryParse(playerOrTargeter, out int id) || !Player.TryGet(id, out Player p)) && !Player.TryGetByName(playerOrTargeter, out p))
            {
                response = $"Player \"{playerOrTargeter}\" does not exist!";

                return false;
            }

            return Action(p, cu, out response);
        }

        public bool Action(Player p, CustomItemBase cu, out string response)
        {
            if (p.IsInventoryFull)
            {
                response = "Target's inventory is full!";

                return false;
            }

            p.GiveCustomItem(cu);

            response = $"Item \"{cu.DisplayName}\" given to ({p.PlayerId}) {p.DisplayNickname}.";

            return true;
        }
    }
}
