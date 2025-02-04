﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics.FixedPoint;
using UnityEngine;

namespace UnitySTG.THSTG.Stage
{
    public class TimeScaleController : MonoBehaviour
    {
        private class Handle : ITimeScaleHandle
        {
            private readonly TimeScaleController timeScaleController;

            private fp _timeScale;

            public fp TimeScale
            {
                get => _timeScale;
                set
                {
                    _timeScale = value;
                    OnTimeScaleChanged?.Invoke(this, EventArgs.Empty);
                }
            }

            public event EventHandler OnTimeScaleChanged;

            public Handle(TimeScaleController timeScaleController)
            {
                this.timeScaleController = timeScaleController;
            }

            public void Dispose()
            {
                OnTimeScaleChanged = null;
                timeScaleController._timeScaleHandles.Remove(this);
            }
        }

        private readonly List<ITimeScaleHandle> _timeScaleHandles = new();

        private void Awake()
        {
            Time.timeScale = 1.0f;
        }

        public ITimeScaleHandle GetTimeScaleHandle()
        {
            var handle = new Handle(this);
            handle.OnTimeScaleChanged += (o, e) => UpdateTimeScale();
            _timeScaleHandles.Add(handle);
            return handle;
        }

        private void UpdateTimeScale()
        {
            fp scale = 1.0M;
            for (var i = 0; i < _timeScaleHandles.Count; i++)
            {
                var handle = _timeScaleHandles[i];
                scale *= handle.TimeScale;
            }
            Time.timeScale = (float)scale;
        }

        public void SetTimeScale(int timeScale)
        {
            Time.timeScale = timeScale;
        }
    }
}
