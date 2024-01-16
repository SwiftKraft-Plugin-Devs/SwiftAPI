﻿using CommandSystem;
using PluginAPI.Core;
using SwiftAPI.API.CustomItems;

namespace SwiftAPI.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Info : CommandBase
    {
        public override string[] GetAliases() => new string[] { "inf", "whatdo", "check", "helpitem" };

        public override string GetCommandName() => "info";

        public override string GetDescription() => "Tells you what the item you are holding does. ";

        public override bool GetRequirePlayer() => true;

        public override PlayerPermissions[] GetPerms() => null;

        public override bool PlayerBasedFunction(Player player, string[] args, out string result)
        {
            if (player.CurrentItem == null)
            {
                result = "Please hold the item you are trying to look up info of! ";

                return false;
            }

            if (CustomItemManager.TryGetCustomItemWithSerial(player.CurrentItem.ItemSerial, out CustomItemBase cust))
            {
                result = $"\n\n<color=#FFFFFF><b>Item Name:</b></color> {cust.DisplayName}\n<color=#FFFFFF><b>Internal ID:</b></color> {cust.CustomItemID}\n\n<color=#FFFFFF><b>Description:</b></color> {cust.Description}";

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
