using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomItemAPI.API
{
    public abstract class CustomItemEquippable : CustomItem
    {
        public abstract void Equip(ushort _itemSerial);
        public abstract void Unequip(ushort _itemSerial);
    }
}
