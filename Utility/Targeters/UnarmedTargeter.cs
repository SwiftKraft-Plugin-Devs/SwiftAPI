using PluginAPI.Core;

namespace SwiftAPI.Utility.Targeters
{
    public class UnarmedTargeter : ArmedTargeter
    {
        public override bool GetAttribute(Player p) => p.IsHuman && !base.GetAttribute(p);

        public override string GetTargeterName() => "UNARMED";

        public override string GetTargeterDescription() => "All human players without a weapon in their inventory.";
    }
}
