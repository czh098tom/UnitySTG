using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.Abstractions.Extension
{
    public static class StageFinishEventBusExtension
    {
        public static IStageFinishEventBus RegisterOnSelf(this IStageFinishEventBus self, EventHandler eventHandler) 
        {
            self.OnFinish += eventHandler;
            return self;
        }
    }
}
