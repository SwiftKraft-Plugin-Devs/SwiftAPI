using PlayerRoles;
using PluginAPI.Core;
using SwiftAPI.Utility.Targeters;
using System;
using System.Collections.Generic;

namespace SwiftAPI.Utility.Spawners
{
    public class PlayerSpawner : SpawnerBase
    {
        public RoleTypeId Pool;
        public TargeterBase Targeter;

        public RoleTypeId Role;

        public override bool SetSpawnee(string[] value, out string feedback)
        {
            value.TryGet(0, out string arg1);
            value.TryGet(1, out string arg2);

            if (TryGetRole(arg1, out Role) && Role != RoleTypeId.None)
            {
                if (!string.IsNullOrEmpty(arg2))
                {
                    if (!TryGetRole(arg2, out Pool) || Pool == RoleTypeId.None)
                        Pool = RoleTypeId.Spectator;

                    TargeterManager.TryGetTargeter(arg2, out Targeter);
                }
                else
                {
                    Pool = RoleTypeId.Spectator;
                    Targeter = null;
                }

                feedback = RoleTranslations.GetRoleName(Role) + " Spawner: " + ToString();

                return true;
            }

            Active = false;

            feedback = "Unknown role ID! Disabled spawner. ";

            return false;
        }

        private bool TryGetRole(string value, out RoleTypeId role)
        {
            if (int.TryParse(value, out int result) && Enum.GetValues(typeof(RoleTypeId)).ToArray<RoleTypeId>().Contains((RoleTypeId)result))
            {
                role = (RoleTypeId)result;

                return true;
            }

            role = RoleTypeId.None;

            return false;
        }

        public override void Spawn()
        {
            List<Player> players;

            if (Targeter == null)
            {
                players = Player.GetPlayers();

                players.RemoveAll((p) => p == null || p.Role != Pool || p.Role == Role);

                Player chosen = players.RandomItem();

                chosen.Role = Role;
                chosen.Position = Position;

                return;
            }

            players = Targeter.GetPlayers();

            players.RandomItem().Role = Role;
        }

        public override string ToString()
        {
            return base.ToString() + "\nSpawn Role: " + RoleTranslations.GetRoleName(Role) + (Targeter == null ? "\nPool Role: " + RoleTranslations.GetRoleName(Pool) + " (" + (int)Pool + ")" : "\nPool Targeter: @" + Targeter.GetTargeterName());
        }
    }
}
