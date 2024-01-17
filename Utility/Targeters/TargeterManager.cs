using PluginAPI.Core;
using PluginAPI.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SwiftAPI.Utility.Targeters
{
    public static class TargeterManager
    {
        public static readonly Dictionary<string, TargeterBase> RegisteredTargeters = new Dictionary<string, TargeterBase>();

        public static void Init()
        {
            InitializeTargeters();
        }

        private static List<TargeterBase> InitializeTargeters()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(assembly => assembly.GetTypes())
        .Where(type => type.IsSubclassOf(typeof(TargeterBase)) && !type.IsAbstract)
        .Select(type => Activator.CreateInstance(type) as TargeterBase).ToList();
        }

        public static bool TryGetTargeterPlayers(string str, out List<Player> players)
        {
            str = str.ToUpper().Replace("@", "");

            if (TryGetTargeter(str, out TargeterBase targ))
            {
                players = targ.GetPlayers();
                return true;
            }

            players = new List<Player>();
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
