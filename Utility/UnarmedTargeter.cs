using PluginAPI.Core;

namespace SwiftAPI.Utility
{
    public class UnarmedTargeter : ArmedTargeter
    {
        public override bool GetAttribute(Player p) => !base.GetAttribute(p);

        public override string GetTargeterName() => "UNARMED";
    }
}
