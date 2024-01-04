using PluginAPI.Core;
using System.Collections.Generic;

namespace CustomItemAPI.Utility
{
    public static class TargeterManager
    {
        public static readonly Dictionary<string, Targeter> RegisteredTargeters = new Dictionary<string, Targeter>();

        public static bool TryGetTargeterPlayers(string str, out List<Player> players)
        {
            str = str.ToUpper();

            if (RegisteredTargeters.ContainsKey(str))
            {
                players = RegisteredTargeters[str].GetPlayers();
                return true;
            }

            players = new List<Player>();
            return false;
        }
    }
}
