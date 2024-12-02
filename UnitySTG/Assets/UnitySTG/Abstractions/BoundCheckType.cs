using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.Abstractions
{
    [Flags]
    public enum BoundCheckType
    {
        XY = 0b000,
        VXY = 0b001,
        //DXY = 0b010,
        //AXY = 0b100,
    }
}
