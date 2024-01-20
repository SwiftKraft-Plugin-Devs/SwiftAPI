using CustomPlayerEffects;
using InventorySystem.Items;
using PluginAPI.Core;

namespace SwiftAPI.API.CustomItems
{
    public class CustomItemConsumableEffect<T> : CustomItemConsumable where T : StatusEffectBase
    {
        public float Duration;
        public bool AddDuration;

        public override void EndConsume(Player _player, ItemBase _item)
        {
            _player.EffectsManager.EnableEffect<T>(Duration, AddDuration);

            base.EndConsume(_player, _item);
        }
    }
}
