using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomItemAPI.API
{
    // Base class for custom items, other plugins can make subclasses of this main class for differently functioning custom items

    public abstract class CustomItem
    {
        public string CustomItemID;

        public abstract void Pickup(ushort _itemSerial);
        public abstract void Drop(ushort _itemSerial);
    }
}
