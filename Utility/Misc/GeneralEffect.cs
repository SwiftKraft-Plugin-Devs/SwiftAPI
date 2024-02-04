using CustomPlayerEffects;
using PluginAPI.Core;

namespace SwiftAPI.Utility.Misc
{
    public abstract class GeneralEffect
    {
        public byte Intensity;
        public float Duration;
        public bool AddDuration;

        public abstract void ApplyEffect(Player p);
    }

    public class GeneralEffect<T> : GeneralEffect where T : StatusEffectBase
    {
        public override void ApplyEffect(Player p) => p.EffectsManager.EnableEffect<T>(Duration, AddDuration).Intensity = Intensity;
    }
}
