using CommandSystem;
using CustomItemAPI.API;
using PluginAPI.Core;
using System;

namespace CustomItemAPI.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class GiveItem : CommandBase
    {
        public override string[] GetAliases()
        {
            return new string[] { "cust" };
        }

        public override string GetCommandName()
        {
            return "customitem";
        }

        public override string GetDescription()
        {
            return "Gives the executor a custom item. ";
        }

        public override PlayerPermissions[] GetPerms()
        {
            return new PlayerPermissions[] { PlayerPermissions.GivingItems };
        }

        public override bool GetRequirePlayer()
        {
            return true;
        }

        public override bool PlayerBasedFunction(Player player, string[] args, out string result)
        {
            if (TryGetArgument(args, 1, out string _arg) && CustomItemManager.TryGetCustomItemWithID(_arg, out CustomItemBase _item))
                return Action(player, out result, _item);

            result = "Inputted custom item does not exist! ";
            return false;
        }

        /// <summary>
        /// Gives the custom item to the player.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="response"></param>
        /// <param name="cu"></param>
        /// <returns></returns>
        public static bool Action(Player p, out string response, CustomItemBase cu)
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
