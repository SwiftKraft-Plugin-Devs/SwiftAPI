using PluginAPI.Core;

namespace SwiftAPI.Utility.Targeters
{
    public class EmptyInventoryTargeter : HumanTargeter
    {
        public override bool GetAttribute(Player p) => base.GetAttribute(p) && p.IsWithoutItems;

        public override string GetTargeterName() => "INVEMPTY";

        public override string GetTargeterDescription() => "All human players without any items.";
    }
}
