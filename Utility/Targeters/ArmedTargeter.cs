using InventorySystem.Items;
using PluginAPI.Core;

namespace SwiftAPI.Utility.Targeters
{
    public class ArmedTargeter : HumanTargeter
    {
        public override bool GetAttribute(Player p)
        {
            foreach (ItemBase it in p.ReferenceHub.inventory.UserInventory.Items.Values)
                if (it.Category == ItemCategory.Firearm)
                    return base.GetAttribute(p);

            return false;
        }

        public override string GetTargeterName() => "ARMED";

        public override string GetTargeterDescription() => "All human players with a weapon in their inventory.";
    }
}
