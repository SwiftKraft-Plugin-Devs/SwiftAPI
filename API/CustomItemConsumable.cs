using InventorySystem.Items;
using PluginAPI.Core;

namespace CustomItemAPI.API
{
    public abstract class CustomItemConsumable : CustomItemBase
    {
        public virtual void StartConsume(Player _player, ItemBase _item)
        {
            ActionHint(_player, "Using");
        }

        public virtual void CancelConsume(Player _player, ItemBase _item)
        {
            ActionHint(_player, "Canceled");
        }

        public virtual void EndConsume(Player _player, ItemBase _item)
        {
            ActionHint(_player, "Used");
        }
    }
}
