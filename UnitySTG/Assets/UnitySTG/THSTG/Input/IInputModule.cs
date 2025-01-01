using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics.FixedPoint;

namespace UnitySTG.THSTG.Input
{
    public interface IInputModule
    {
        public Span<InputAxis> ConvertAxis();
    }
}
