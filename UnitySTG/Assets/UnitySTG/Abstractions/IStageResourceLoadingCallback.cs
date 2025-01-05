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
        public UniTask<IResourceDictionary> LoadResources(IProgress<float> progress, CancellationToken cancellationToken);
    }
}
