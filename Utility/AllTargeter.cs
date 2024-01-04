using PluginAPI.Core;
using System.Collections.Generic;

namespace CustomItemAPI.Utility
{
    public class AllTargeter : Targeter
    {
        public override List<Player> GetPlayers()
        {
            return Player.GetPlayers();
        }

        public override string GetTargeterName() => "ALL";
    }
}
