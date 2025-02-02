using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.Abstractions
{
    public class StageFinishEventBus : IStageFinishEventBus
    {
        public event EventHandler OnFinish;

        public void RaiseOnFinish(object sender, EventArgs args)
        {
            OnFinish?.Invoke(sender, args);
        }
    }
}
