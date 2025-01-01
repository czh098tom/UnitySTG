using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.Abstractions
{
    public class StageDescriptor
    {
        public IStageResourceLoadingCallback StageResourceLoadingCallback { get; private set; }
        public IStageInitializationCallback StageInitializationCallback { get; private set; }
        public IStageFrameCallback StageFrameCallback { get; private set; }
        public IStageFinishCallback StageFinishCallback { get; private set; }

        public StageDescriptor(
            IStageResourceLoadingCallback stageResourceLoadingCallback, 
            IStageInitializationCallback stageInitializationCallback, 
            IStageFrameCallback stageFrameCallback, 
            IStageFinishCallback stageFinishCallback)
        {
            StageResourceLoadingCallback = stageResourceLoadingCallback;
            StageInitializationCallback = stageInitializationCallback;
            StageFrameCallback = stageFrameCallback;
            StageFinishCallback = stageFinishCallback;
        }
    }
}
