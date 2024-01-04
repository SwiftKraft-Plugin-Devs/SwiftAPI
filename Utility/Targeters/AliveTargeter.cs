using PluginAPI.Core;
namespace SwiftAPI.Utility.Targeters
{
    public class AliveTargeter : PlayerAttributeTargeter
    {
        public override bool GetAttribute(Player p) => p.IsAlive;

        public override string GetTargeterName() => "ALIVE";
    }
}
