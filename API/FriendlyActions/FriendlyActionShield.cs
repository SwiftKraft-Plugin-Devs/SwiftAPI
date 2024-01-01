using PlayerStatsSystem;
using PluginAPI.Core;
using UnityEngine;

namespace CustomItemAPI.API.FriendlyActions
{
    public class FriendlyActionShield : FriendlyAction
    {
        public float Amount;
        public float Decay;
        public float Limit;
        public float Efficacy;
        public float Sustain;
        public bool Persistent;

        private int processIndex;

        public override void Hit(Player _player, Player _target)
        {
            if (_target.ReferenceHub.playerStats.StatModules[1] is AhpStat ahp)
            {
                if (ahp._activeProcesses.Count <= processIndex)
                {
                    AhpStat.AhpProcess process = ahp.ServerAddProcess(Amount, Limit, Decay, Efficacy, Sustain, Persistent);
                    processIndex = ahp._activeProcesses.IndexOf(process);
                }
                else
                {
                    AhpStat.AhpProcess prev = ahp._activeProcesses[processIndex];
                    ahp._activeProcesses[processIndex] = new AhpStat.AhpProcess(Mathf.Clamp(prev.CurrentAmount + Amount, 0f, Limit), Limit, Decay, Efficacy, Sustain, Persistent);
                }
            }
        }
    }
}
