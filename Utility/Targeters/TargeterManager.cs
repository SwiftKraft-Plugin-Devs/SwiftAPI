using PluginAPI.Core;
using System.Collections.Generic;

namespace SwiftAPI.Utility.Targeters
{
    public static class TargeterManager
    {
        public static readonly Dictionary<string, TargeterBase> RegisteredTargeters = new Dictionary<string, TargeterBase>();

        public static bool TryGetTargeterPlayers(string str, out List<Player> players)
        {
            str.Replace("@", "");
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
