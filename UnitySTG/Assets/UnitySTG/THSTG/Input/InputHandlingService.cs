using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics.FixedPoint;
using UnityEngine;

using UnitySTG.Abstractions;

namespace UnitySTG.THSTG.Input
{
    public class InputHandlingService : MonoBehaviour, IStageFrameCallback
    {
        public IInputModule InputModule { get; set; }
        public IReplayReader ReplayReader { get; set; }
        public IReplayWriter ReplayWriter { get; set; }

        private readonly Queue<InputAxis> pendingInput = new();
        private readonly Dictionary<int, fp> currentAxisValue = new();
        private readonly HashSet<int> openedAxis = new();

        public void OpenAxis(int axisIndex)
        {
            currentAxisValue.Add(axisIndex, 0M);
            openedAxis.Add(axisIndex);
        }

        public void OnFrame(ILevelServiceProvider levelServiceProvider)
        {
            foreach (var key in openedAxis)
            {
                currentAxisValue[key] = 0M;
            }
            InputAxis[] axes = new InputAxis[pendingInput.Count];
            var pendingID = 0;
            while (pendingInput?.TryDequeue(out var axis) ?? false)
            {
                if (openedAxis.Contains(axis.AxisIndex))
                {
                    currentAxisValue[axis.AxisIndex] = axis.Value;
                }
                axes[pendingID] = axis;
                pendingID++;
            }
            ReplayWriter?.Write(new Span<InputAxis>(axes));
            if (ReplayReader != null)
            {
                foreach (var axis in ReplayReader.OnNext())
                {
                    if (openedAxis.Contains(axis.AxisIndex))
                    {
                        currentAxisValue[axis.AxisIndex] = axis.Value;
                    }
                }
            }
        }

        public fp GetAxis(int axisIndex)
        {
            return openedAxis.Contains(axisIndex) ? currentAxisValue.GetValueOrDefault(axisIndex, 0M) : 0M;
        }

        private void Update()
        {
            if (InputModule != null)
            {
                foreach (var axises in InputModule.ConvertAxis())
                {
                    pendingInput.Enqueue(axises);
                }
            }
        }
    }
}
