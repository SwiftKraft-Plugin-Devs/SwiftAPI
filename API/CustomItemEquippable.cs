using Hints;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomItemAPI.API
{
    /// <summary>
    /// Equippable custom items base class. 
    /// </summary>
    public class CustomItemEquippable : CustomItemBase
    {
        /// <summary>
        /// Called when equipping item. 
        /// </summary>
        /// <param name="_player"></param>
        /// <param name="_itemSerial"></param>
        public virtual void Equip(Player _player, ushort _itemSerial)
        {
            _player.ReceiveHint("Equipped: " + DisplayName, new HintEffect[] { HintEffectPresets.FadeOut() }, 3f);
        }

        /// <summary>
        /// Called when unequipping item.
        /// </summary>
        /// <param name="_player"></param>
        /// <param name="_itemSerial"></param>
        public virtual void Unequip(Player _player, ushort _itemSerial) { }

        public override void Init(ushort _itemSerial) { }
        public override void Destroy(ushort _itemSerial) { }
    }
}
