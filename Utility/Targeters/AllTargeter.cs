using PluginAPI.Core;
using System.Collections.Generic;

namespace SwiftAPI.Utility.Targeters
{
    public class AllTargeter : TargeterBase
    {
        public override List<Player> GetPlayers()
        {
            return UtilityFunctions.GetPlayersIgnoreJoin();
        }

        public override string GetTargeterName() => "ALL";

        public override string GetTargeterDescription() => "All players.";
    }
}
