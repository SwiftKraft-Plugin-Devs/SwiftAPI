using PluginAPI.Core;
using System.Collections.Generic;

namespace CustomItemAPI.Utility
{
    public abstract class Targeter
    {
        public string TargeterName;

        public Targeter(string name)
        {
            TargeterName = name;
            Targeters.RegisteredTargeters.Add(this);
        }

        public abstract List<Player> GetPlayers();
    }
}
