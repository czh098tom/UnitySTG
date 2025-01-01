using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.Abstractions
{
    public interface IStageInitializationCallback
    {
        public void OnInit(ILevelServiceProvider levelServiceProvider);
    }
}
