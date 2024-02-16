using CommandSystem;
using PluginAPI.Core;
using SwiftAPI.API.BreakableToys;
using SwiftAPI.API.CustomItems;
using SwiftAPI.Utility.Misc;
using System;
using UnityEngine;

namespace SwiftAPI.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SpawnBreakableToy : CommandBase
    {
        public override string[] GetAliases() => new string[] { "spnbrkt", "brkble", "breakable", "toyb" };

        public override string GetCommandName() => "spawnbreakable";

        public override string GetDescription() => "Spawns a breakable primitive object at your position.";

        public override PlayerPermissions[] GetPerms() => new PlayerPermissions[] { PlayerPermissions.FacilityManagement };

        public override bool GetRequirePlayer() => true;

        public override bool PlayerBasedFunction(Player player, string[] args, out string result)
        {
            if (!TryGetArgument(args, 1, out string arg1) || !Enum.TryParse(arg1, ignoreCase: true, out SnappingModes modes))
            {
                result = "Please input a valid snapping mode! (none, grid and norot)";

                return false;
            }

            if (!TryGetArgument(args, 2, out string arg2) || !Enum.TryParse(arg2, ignoreCase: true, out PrimitiveType type))
            {
                result = "Please input a valid primitive type! ";

                return false;
            }

            if (!TryGetArgument(args, 3, out string arg3) || !float.TryParse(arg3, out float x))
            {
                result = "Please input a number for x scale! ";

                return false;
            }

            if (!TryGetArgument(args, 4, out string arg4) || !float.TryParse(arg4, out float y))
            {
                result = "Please input a number for y scale! ";

                return false;
            }

            if (!TryGetArgument(args, 5, out string arg5) || !float.TryParse(arg5, out float z))
            {
                result = "Please input a number for z scale! ";

                return false;
            }

            if (!TryGetArgument(args, 6, out string arg6) || !ColorUtility.TryParseHtmlString(arg6, out Color color))
            {
                result = "Please input an HTML code for color! ";

                return false;
            }

            if (!TryGetArgument(args, 7, out string arg7) || !float.TryParse(arg7, out float hp))
            {
                result = "Please input a number for health! ";

                return false;
            }

            Vector3 position = player.Position;
            Quaternion rotation = Quaternion.Euler(player.Rotation);
            Vector3 scale = new(x, y, z);

            switch (modes)
            {
                case SnappingModes.Grid:
                    rotation = Quaternion.identity;
                    position = GridSnapper.SnapToGrid(position, scale, Vector3.one);
                    break;
                case SnappingModes.NoRot:
                    rotation = Quaternion.identity;
                    break;
                default:
                    break;
            }

            BreakableToyBase toy = player.ReferenceHub.SpawnBreakableToy<BreakableToyBase>(type, position, rotation, scale, color);
            toy.SetHealth(hp);

            if (TryGetArgument(args, 8, out string arg8))
            {
                if (int.TryParse(arg8, out int itemId) && Enum.GetValues(typeof(ItemType)).ToArray<ItemType>().Contains((ItemType)itemId))
                    toy.DropItem = (ItemType)itemId;
                else if (CustomItemManager.TryGetCustomItemWithID(arg8, out CustomItemBase custItem))
                    toy.DropCustomItem = custItem;
            }

            result = "Spawned breakable primitive with NetID of " + toy.NetworkId + " and health of " + hp;

            return true;
        }
    }
}

namespace SwiftAPI.API.BreakableToys
{
    public enum SnappingModes : int
    {
        None = 0,
        Grid = 1,
        NoRot = 2,
        DontMove = 3
    }
}
