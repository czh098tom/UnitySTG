using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace UnitySTG.Abstractions
{
    public interface IStageResourceLoadingCallback
    {
        public UniTask<IResourceDictionary> LoadResources(IProgress<ResourceLoadInfo> progress, CancellationToken cancellationToken);
        public float EstimatedLoadingTaskWeight { get; }
    }
}
