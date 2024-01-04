using PluginAPI.Core;

namespace SwiftAPI.Utility.Targeters
{
    public class UnarmedTargeter : ArmedTargeter
    {
        public override bool GetAttribute(Player p) => p.IsHuman && !base.GetAttribute(p);

        public override string GetTargeterName() => "UNARMED";
    }
}
