﻿using Hints;
using PluginAPI.Core;

namespace CustomItemAPI.API.FriendlyActions
{
    public class FriendlyActionHeal : FriendlyAction
    {
        public float Amount;

        public override void Hit(Player _player, Player _target)
        {
            if (_target.Health < _target.MaxHealth)
                _target.Heal(Amount);

            _player.ReceiveHint("Healed " + _target.DisplayNickname + ": <color=#00FF00>+" + (int)Amount + " HP</color>\nTheir Health: <color=#00FF00>" + (int)_target.Health + "/" + (int)_target.MaxHealth + "</color>", new HintEffect[] { HintEffectPresets.FadeOut() }, 1f);
            _target.ReceiveHint("Healing From " + _player.DisplayNickname + ": <color=#00FF00>+" + (int)Amount + " HP</color>", new HintEffect[] { HintEffectPresets.FadeOut() }, 1f);
        }
    }
}
