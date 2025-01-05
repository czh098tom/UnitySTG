using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace UnitySTG.Abstractions
{
    public class StageResourceLoadingCallback : IStageResourceLoadingCallback
    {
        public static IStageResourceLoadingCallback Empty = new StageResourceLoadingCallback(
            (_, _) => UniTask.FromResult<IResourceDictionary>(null));

        private readonly Func<IProgress<float>, CancellationToken, UniTask<IResourceDictionary>> _func;

        private StageResourceLoadingCallback(Func<IProgress<float>, CancellationToken, UniTask<IResourceDictionary>> func) 
        {
            _func = func;
        }

        public UniTask<IResourceDictionary> LoadResources(IProgress<float> progress, CancellationToken cancellationToken)
        {
            return _func(progress, cancellationToken);
        }
    }
}
