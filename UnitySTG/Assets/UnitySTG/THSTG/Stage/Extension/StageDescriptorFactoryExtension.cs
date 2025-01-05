using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnitySTG.Abstractions;
using UnitySTG.Abstractions.ObjectPoolExtension;
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
    }
}
