using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnitySTG.Abstractions;

namespace UnitySTG.THSTG.Bullet
{
    public interface IBulletStyleSubState
    {
        public IDisposable OnIntoState(ILevelServiceProvider levelServiceProvider, BulletController controller);
        public int? GetDuration();
    }
}
