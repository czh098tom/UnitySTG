using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnitySTG.Abstractions;

namespace UnitySTG.THSTG.Bullet
{
    public interface IBulletStyleSheet
    {
        public IObjectStyle BaseStyle { get; }
        public float A { get; }
        public float B { get; }
        public IBulletStyleSubState GetSubState(BulletState state);
    }
}
