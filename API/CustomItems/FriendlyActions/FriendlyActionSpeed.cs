using CustomPlayerEffects;
using Hints;
using PluginAPI.Core;
using UnityEngine;

namespace SwiftAPI.API.CustomItems.FriendlyActions
{
    public class FriendlyActionSpeed : FriendlyAction
    {
        public byte Intensity;

        public float Duration;

        public override void Hit(Player _player, Player _target)
        {
            _player.EffectsManager.EnableEffect<MovementBoost>(Duration, false).Intensity = Intensity;
            _target.EffectsManager.EnableEffect<MovementBoost>(Duration, false).Intensity = Intensity;
            _player.ReceiveHint("Speed Boosted " + _target.DisplayNickname, [HintEffectPresets.FadeOut()], Mathf.Min(Duration, 4f));
            _target.ReceiveHint("Speed Boost From " + _player.DisplayNickname, [HintEffectPresets.FadeOut()], Mathf.Min(Duration, 4f));
        }
    }
}
