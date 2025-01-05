using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.Abstractions
{
    public record StageDescriptor(
        IStageResourceLoadingCallback StageResourceLoadingCallback,
        IStageInitializationCallback StageInitializationCallback,
        IStageFrameCallback StageFrameCallback,
        IStageFinishCallback StageFinishCallback);
}
