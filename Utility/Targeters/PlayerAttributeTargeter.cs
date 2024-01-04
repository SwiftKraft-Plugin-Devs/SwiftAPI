using PluginAPI.Core;
using System.Collections.Generic;

namespace SwiftAPI.Utility.Targeters
{
    public abstract class PlayerAttributeTargeter : AllTargeter
    {
        public abstract bool GetAttribute(Player p);

        public override List<Player> GetPlayers()
        {
            List<Player> temp = base.GetPlayers();
            temp.RemoveAll((p) => !GetAttribute(p));
            return temp;
        }
    }
}
