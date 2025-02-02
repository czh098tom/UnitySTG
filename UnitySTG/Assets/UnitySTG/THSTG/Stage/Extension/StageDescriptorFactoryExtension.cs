using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnitySTG.Abstractions;
using UnitySTG.Abstractions.Extension;
using UnitySTG.THSTG.Input;

namespace UnitySTG.THSTG.Stage.Extension
{
    public static class StageDescriptorFactoryExtension
    {
        public static StageDescriptorFactory AddInputHandling(this StageDescriptorFactory factory,
            Action<ILevelServiceProvider, InputHandlingService> configure = null)
        {
            return factory.AppendStageInit(configure)
                .PrependStageFrameCached<InputHandlingService>();
        }

        public static StageDescriptorFactory AddStageSegment(this StageDescriptorFactory factory,
            IStageSegment stageSegment)
        {
            return factory
                .ModifyStageResourceLoading(i => i.Append(stageSegment.EstimatedLoadingTaskWeight, stageSegment.LoadResources))
                .ModifyStageInit(i => i.Append(l => stageSegment.OnInit(l)))
                .ModifyStageFrame(i => i.Append(l => stageSegment.OnFrame(l)))
                .ModifyStageFinish(i => stageSegment.RegisterOnSelf((o, e) => i.RaiseOnFinish(o, e)));
        }
    }
}
