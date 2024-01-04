using PluginAPI.Core;
using System.Collections.Generic;

namespace SwiftAPI.Utility.Targeters
{
    public abstract class Targeter
    {
        public Targeter()
        {
            if (!TargeterManager.RegisteredTargeters.ContainsKey(GetTargeterName().ToUpper()))
                TargeterManager.RegisteredTargeters.Add(GetTargeterName().ToUpper(), this);
        }

        public abstract string GetTargeterName();

        public abstract List<Player> GetPlayers();
    }
}
