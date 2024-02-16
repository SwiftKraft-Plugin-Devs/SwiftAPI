using CommandSystem;
using SwiftAPI.Utility.Spawners;

namespace SwiftAPI.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class DeleteSpawner : CommandBase
    {
        public override string[] GetAliases() => ["dsp"];

        public override string GetCommandName() => "delsp";

        public override string GetDescription() => "Deletes a spawner. ";

        public override PlayerPermissions[] GetPerms() => [PlayerPermissions.GivingItems];

        public override bool Function(string[] args, ICommandSender sender, out string result)
        {
            if (!TryGetArgument(args, 1, out string arg1) || !int.TryParse(arg1, out int id))
            {
                result = "Please input a Spawner ID! ";

                return false;
            }

            if (SpawnerManager.RemoveSpawner(id))
            {
                result = "Removed spawner with ID: " + id;

                return true;
            }
            else
            {
                result = "Out of range! ";

                return false;
            }
        }
    }
}
