using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitySTG.Abstractions;

namespace UnitySTG.THSTG.Stage
{
    public interface IStageSegment : IStageResourceLoadingCallback, IStageInitializationCallback, IStageFrameCallback, IStageFinishEventBus
    {
    }
}
