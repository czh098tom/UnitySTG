using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unity.Mathematics.FixedPoint;

namespace UnitySTG.THSTG.Stage
{
    public interface ITimeScaleHandle : IDisposable
    {
        public event EventHandler OnTimeScaleChanged;
        public fp TimeScale { get; set; }
    }
}
