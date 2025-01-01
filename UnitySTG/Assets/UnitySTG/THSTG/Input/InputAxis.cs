using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics.FixedPoint;

namespace UnitySTG.THSTG.Input
{
    public struct InputAxis
    {
        public int AxisIndex { get; private set; }
        public fp Value { get; private set; }

        public InputAxis(int axisIndex, fp value)
        {
            AxisIndex = axisIndex;
            Value = value;
        }
    }
}
