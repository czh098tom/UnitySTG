using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

using UnitySTG.Abstractions;

namespace UnitySTG.THSTG.Stage
{
    public abstract class StageSegmentBase : IStageSegment
    {

        public event EventHandler OnFinish;

        public abstract float EstimatedLoadingTaskWeight { get; }

        public abstract UniTask<IResourceDictionary> LoadResources(IProgress<ResourceLoadInfo> progress, CancellationToken cancellationToken);

        public abstract void OnInit(ILevelServiceProvider levelServiceProvider);

        public abstract void OnFrame(ILevelServiceProvider levelServiceProvider);

        protected void RaiseSegmentFinish(object sender, EventArgs args)
        {
            OnFinish?.Invoke(this, EventArgs.Empty);
        }

        void IStageFinishEventBus.RaiseOnFinish(object sender, EventArgs args)
        {
            OnFinish?.Invoke(this, EventArgs.Empty);
        }
    }
}
