using PluginAPI.Core;
using System.Collections.Generic;

namespace CustomItemAPI.Utility
{
    public static class Targeters
    {
        public static readonly List<Targeter> RegisteredTargeters = new List<Targeter>();

        public static bool TryGetTargeterPlayers(string str, out List<Player> players)
        {
            foreach (Targeter t in RegisteredTargeters)
            {
                if (t.TargeterName == str)
                {
                    players = t.GetPlayers();
                    return true;
                }
            }

            players = new List<Player>();
            return false;
        }
    }
}
