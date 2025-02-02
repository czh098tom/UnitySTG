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
    public class StageSegmentCollection : StageSegmentBase
    {
        private readonly IStageSegment[] _stageSegments;

        private int _index = -1;
        private ILevelServiceProvider _levelServiceProvider;

        public override float EstimatedLoadingTaskWeight { get; }

        public StageSegmentCollection(IStageSegment[] stageSegments)
        {
            _stageSegments = stageSegments;
            EstimatedLoadingTaskWeight = 0;
            for (int i = 0; i < stageSegments.Length; i++)
            {
                EstimatedLoadingTaskWeight += stageSegments[i].EstimatedLoadingTaskWeight;
            }
        }

        public override async UniTask<IResourceDictionary> LoadResources(IProgress<ResourceLoadInfo> progress, CancellationToken cancellationToken)
        {
            var dict = new ResourceDictionary();
            foreach (var segment in _stageSegments)
            {
                dict.MergeDictionary(await segment.LoadResources(progress, cancellationToken));
            }
            return dict;
        }

        public override void OnInit(ILevelServiceProvider levelServiceProvider)
        {
            _levelServiceProvider = levelServiceProvider;
            TryProceedToNext(this, EventArgs.Empty);
        }

        public override void OnFrame(ILevelServiceProvider levelServiceProvider)
        {
            if (_index >= 0 && _index < _stageSegments.Length)
            {
                var curr = _stageSegments[_index];
                curr.OnFrame(levelServiceProvider);
            }
        }

        private void Segment_OnFinish(object sender, EventArgs e)
        {
            TryProceedToNext(sender, e);
        }

        private void TryProceedToNext(object sender, EventArgs e)
        {
            if (_index >= 0)
            {
                _stageSegments[_index].OnFinish -= Segment_OnFinish;
            }
            _index++;
            if (_index < _stageSegments.Length)
            {
                _stageSegments[_index].OnInit(_levelServiceProvider);
                _stageSegments[_index].OnFinish += Segment_OnFinish;
            }
            else
            {
                RaiseOnFinish(sender, e);
            }
        }
    }
}
