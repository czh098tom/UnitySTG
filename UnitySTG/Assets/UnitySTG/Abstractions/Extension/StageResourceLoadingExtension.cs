using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitySTG.Abstractions.Extension
{
    public static class StageResourceLoadingExtension
    {
        public static IStageResourceLoadingCallback Prepend(this IStageResourceLoadingCallback callback, float estimatedTaskWeight, Func<IProgress<ResourceLoadInfo>, CancellationToken, UniTask<IResourceDictionary>> func)
        {
            return StageResourceLoadingCallback.Prepend(callback, estimatedTaskWeight, func);
        }

        public static IStageResourceLoadingCallback Append(this IStageResourceLoadingCallback callback, float estimatedTaskWeight, Func<IProgress<ResourceLoadInfo>, CancellationToken, UniTask<IResourceDictionary>> func)
        {
            return StageResourceLoadingCallback.Append(callback, estimatedTaskWeight, func);
        }
    }
}
