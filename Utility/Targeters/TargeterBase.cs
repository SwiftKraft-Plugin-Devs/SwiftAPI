using PluginAPI.Core;
using System.Collections.Generic;

namespace SwiftAPI.Utility.Targeters
{
    public abstract class TargeterBase
    {
        public TargeterBase()
        {
            if (!TargeterManager.RegisteredTargeters.ContainsKey(GetTargeterName().ToUpper()))
                TargeterManager.RegisteredTargeters.Add(GetTargeterName().ToUpper(), this);
        }

        public abstract string GetTargeterName();

        public virtual string GetTargeterDescription() => "No description.";

        public abstract List<Player> GetPlayers();
    }
}
