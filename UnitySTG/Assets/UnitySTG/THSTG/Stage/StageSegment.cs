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
    public class StageSegment : StageSegmentBase
    {
        public override float EstimatedLoadingTaskWeight => 0;

        public override UniTask<IResourceDictionary> LoadResources(IProgress<ResourceLoadInfo> progress, CancellationToken cancellationToken)
        {
            return UniTask.FromResult<IResourceDictionary>(ResourceDictionary.Empty);
        }

        public override void OnInit(ILevelServiceProvider levelServiceProvider)
        {
        }

        public override void OnFrame(ILevelServiceProvider levelServiceProvider)
        {
        }
    }
}
