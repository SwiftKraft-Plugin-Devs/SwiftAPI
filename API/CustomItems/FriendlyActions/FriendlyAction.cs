using PluginAPI.Core;

namespace SwiftAPI.API.CustomItems.FriendlyActions
{
    public abstract class FriendlyAction
    {
        public abstract void Hit(Player _player, Player _target);
    }
}
