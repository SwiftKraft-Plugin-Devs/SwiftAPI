using PluginAPI.Core;
using System.Collections.Generic;

namespace SwiftAPI.Utility
{
    public static class UtilityFunctions
    {
        public static List<Player> GetPlayersIgnoreJoin()
        {
            List<Player> players = Player.GetPlayers();
            players.RemoveAll((p) => EventHandler.JoiningPlayers.Contains(p));
            return players;
        }
    }
}
