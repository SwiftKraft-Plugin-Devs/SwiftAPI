using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SwiftAPI.Utility.Targeters
{
    public static class TargeterManager
    {
        public static readonly Dictionary<string, TargeterBase> RegisteredTargeters = [];

        public static bool TryGetTargeterPlayers(string str, out List<Player> players)
        {
            str = str.ToUpper().Replace("@", "");

            if (TryGetTargeter(str, out TargeterBase targ))
            {
                players = targ.GetPlayers();
                return true;
            }

            players = [];
            return false;
        }

        public static bool TryGetTargeter(string str, out TargeterBase targ)
        {
            str = str.ToUpper().Replace("@", "");

            if (RegisteredTargeters.ContainsKey(str))
            {
                targ = RegisteredTargeters[str];
                return true;
            }

            targ = null;
            return false;
        }
    }
}
