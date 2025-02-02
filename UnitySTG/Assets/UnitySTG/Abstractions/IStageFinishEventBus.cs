using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.Abstractions
{
    public interface IStageFinishEventBus
    {
        public event EventHandler OnFinish;
        public void RaiseOnFinish(object sender, EventArgs args);
    }
}
