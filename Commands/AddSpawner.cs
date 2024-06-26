﻿using CommandSystem;
using PluginAPI.Core;
using SwiftAPI.Utility.Spawners;
using System.Collections.Generic;

namespace SwiftAPI.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class AddSpawner : CommandBase
    {
        public override string[] GetAliases() => ["asp"];

        public override string GetCommandName() => "addsp";

        public override string GetDescription() => "Adds a spawner of any implemented type.";

        public override PlayerPermissions[] GetPerms() => [PlayerPermissions.GivingItems];

        public override bool GetRequirePlayer() => true;

        public override bool PlayerBasedFunction(Player player, string[] args, out string result)
        {
            if (!TryGetArgument(args, 1, out string arg1) || !SpawnerManager.IsValidType(arg1))
            {
                result = $"Spawner type \"{arg1}\" not found! ";

                return false;
            }

            if (!TryGetArgument(args, 2, out string arg2) || !float.TryParse(arg2, out float time) || time <= 0f)
            {
                result = $"{arg2} is not a valid number for timer! ";

                return false;
            }

            if (args.Length < 4)
            {
                result = "Please input the custom data (such as item ID) the spawner needs! ";

                return false;
            }

            SpawnerBase sp = SpawnerManager.AddSpawner(player.Position, time, arg1);

            List<string> customData = [.. args];
            customData.RemoveRange(0, 3);

            if (!sp.SetSpawnee([.. customData], out result))
            {
                SpawnerManager.RemoveSpawner(sp.ID);

                return false;
            }
            else
                return true;
        }
    }
}
