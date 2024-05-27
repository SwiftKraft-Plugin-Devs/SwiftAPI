using PluginAPI.Core;
using System.Collections.Generic;

namespace SwiftAPI.Utility.Targeters
{
    public class AllTargeter : TargeterBase
    {
        public override List<Player> GetPlayers() => Player.GetPlayers();

        public override string GetTargeterName() => "ALL";

        public override string GetTargeterDescription() => "All players.";
    }
}
