using CommandSystem;
using SwiftAPI.API;
using SwiftAPI.Utility;
using PluginAPI.Core;
using System.Collections.Generic;

namespace SwiftAPI.Commands
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
            return "Gives the executor a custom item. Item IDs are CASE SENSITIVE!";
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
            if (TryGetArgument(args, 1, out string _arg1) && CustomItemManager.TryGetCustomItemWithID(_arg1, out CustomItemBase _item))
            {
                if (TryGetArgument(args, 2, out string _arg2) && TargeterManager.TryGetTargeterPlayers(_arg2, out List<Player> players))
                {
                    int accu = 0;
                    for (int i = 0; i < players.Count; i++)
                        if (Action(players[i], out _, _item))
                            accu++;

                    result = $"Item \"{_item.DisplayName}\" given to {accu} players.";

                    return true;
                }
                else
                    return Action(player, out result, _item);
            }

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
