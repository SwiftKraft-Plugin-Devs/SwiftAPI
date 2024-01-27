using CommandSystem;
using PluginAPI.Core;
using SwiftAPI.API.BreakableToys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Vector3 scale = new Vector3(x, y, z);

            switch (modes)
            {
                case SnappingModes.Grid:
                    rotation = Quaternion.identity;
                    position = new Vector3(Mathf.Round(position.x) + (scale.x % 2 != 0 ? 0.5f : 0f), Mathf.Round(position.y) + (scale.y % 2 == 0 ? 0.5f : 0f), Mathf.Round(position.z) + (scale.z % 2 == 0 ? 0.5f : 0f));
                    break;
                case SnappingModes.NoRot:
                    rotation = Quaternion.identity;
                    break;
                default:
                    break;
            }

            BreakableToyBase toy = player.ReferenceHub.SpawnBreakableToy(type, position, rotation, scale, color);
            toy.Configure(hp);

            result = "Spawned breakable primitive with NetID of " + toy.NetworkId + " and health of " + hp;

            return true;
        }

        public enum SnappingModes : int
        {
            None = 0,
            Grid = 1,
            NoRot = 2
        }
    }
}
