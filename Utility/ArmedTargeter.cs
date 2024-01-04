using InventorySystem.Items;
using PluginAPI.Core;

namespace CustomItemAPI.Utility
{
    public class ArmedTargeter : PlayerAttributeTargeter
    {
        public override bool GetAttribute(Player p)
        {
            foreach (ItemBase it in p.ReferenceHub.inventory.UserInventory.Items.Values)
                if (it.Category == ItemCategory.Firearm)
                    return true;

            return false;
        }

        public override string GetTargeterName() => "ARMED";
    }
}
