using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnitySTG.Abstractions.Progress;

namespace UnitySTG.Abstractions
{
    public class StageResourceLoadingCallback : IStageResourceLoadingCallback
    {
        public static IStageResourceLoadingCallback Empty = new StageResourceLoadingCallback(0, 
            (_, _) => UniTask.FromResult<IResourceDictionary>(ResourceDictionary.Empty));

        private class AppendCallback : IStageResourceLoadingCallback
        {
            private readonly IStageResourceLoadingCallback _inner;
            private readonly float _selfEstimatedTaskWeight;
            private readonly Func<IProgress<ResourceLoadInfo>, CancellationToken, UniTask<IResourceDictionary>> _func;

            public float EstimatedLoadingTaskWeight => _selfEstimatedTaskWeight + _inner.EstimatedLoadingTaskWeight;

            public AppendCallback(IStageResourceLoadingCallback inner, float estimatedTaskWeight, 
                Func<IProgress<ResourceLoadInfo>, CancellationToken, UniTask<IResourceDictionary>> func)
            {
                _inner = inner;
                _selfEstimatedTaskWeight = estimatedTaskWeight;
                _func = func;
            }

            public async UniTask<IResourceDictionary> LoadResources(IProgress<ResourceLoadInfo> progress, CancellationToken cancellationToken)
            {
                var dict = new ResourceDictionary();
                dict.MergeDictionary(await _inner.LoadResources(new OffsetProgress(progress, 0, _inner.EstimatedLoadingTaskWeight / EstimatedLoadingTaskWeight), cancellationToken));
                if (_func != null)
                {
                    dict.MergeDictionary(await _func.Invoke(new OffsetProgress(progress, _inner.EstimatedLoadingTaskWeight / EstimatedLoadingTaskWeight, 1), cancellationToken));
                }
                return dict;
            }
        }

        private class PrependCallback : IStageResourceLoadingCallback
        {
            private readonly IStageResourceLoadingCallback _inner;
            private readonly float _selfEstimatedTaskWeight;
            private readonly Func<IProgress<ResourceLoadInfo>, CancellationToken, UniTask<IResourceDictionary>> _func;

            public float EstimatedLoadingTaskWeight => _selfEstimatedTaskWeight + _inner.EstimatedLoadingTaskWeight;

            public PrependCallback(IStageResourceLoadingCallback inner, float estimatedTaskWeight,
                Func<IProgress<ResourceLoadInfo>, CancellationToken, UniTask<IResourceDictionary>> func)
            {
                _inner = inner;
                _selfEstimatedTaskWeight = estimatedTaskWeight;
                _func = func;
            }

            public async UniTask<IResourceDictionary> LoadResources(IProgress<ResourceLoadInfo> progress, CancellationToken cancellationToken)
            {
                var dict = new ResourceDictionary();
                if (_func != null)
                {
                    dict.MergeDictionary(await _func.Invoke(new OffsetProgress(progress, _inner.EstimatedLoadingTaskWeight / EstimatedLoadingTaskWeight, 1), cancellationToken));
                }
                dict.MergeDictionary(await _inner.LoadResources(new OffsetProgress(progress, 0, _inner.EstimatedLoadingTaskWeight / EstimatedLoadingTaskWeight), cancellationToken));
                return dict;
            }
        }

        public static IStageResourceLoadingCallback Create(float estimatedTaskWeight, Func<IProgress<ResourceLoadInfo>, CancellationToken, UniTask<IResourceDictionary>> func)
        {
            return new StageResourceLoadingCallback(estimatedTaskWeight, func);
        }

        public static IStageResourceLoadingCallback Append(IStageResourceLoadingCallback inner, float estimatedTaskWeight, Func<IProgress<ResourceLoadInfo>, CancellationToken, UniTask<IResourceDictionary>> func)
        {
            return new AppendCallback(inner, estimatedTaskWeight, func);
        }

        public static IStageResourceLoadingCallback Prepend(IStageResourceLoadingCallback inner, float estimatedTaskWeight, Func<IProgress<ResourceLoadInfo>, CancellationToken, UniTask<IResourceDictionary>> func)
        {
            return new PrependCallback(inner, estimatedTaskWeight, func);
        }

        private readonly Func<IProgress<ResourceLoadInfo>, CancellationToken, UniTask<IResourceDictionary>> _func;

        public float EstimatedLoadingTaskWeight { get; }

        private StageResourceLoadingCallback(float estimatedTaskWeight, Func<IProgress<ResourceLoadInfo>, CancellationToken, UniTask<IResourceDictionary>> func) 
        {
            EstimatedLoadingTaskWeight = estimatedTaskWeight;
            _func = func;
        }

        public UniTask<IResourceDictionary> LoadResources(IProgress<ResourceLoadInfo> progress, CancellationToken cancellationToken)
        {
            return _func(progress, cancellationToken);
        }
    }
}
