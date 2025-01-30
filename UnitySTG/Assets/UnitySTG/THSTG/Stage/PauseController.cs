using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnitySTG.Abstractions;

namespace UnitySTG.THSTG.Stage
{
    public class PauseController : MonoBehaviour, IStageInitializationCallback
    {
        [SerializeField] private UnityEvent<bool> onPauseStateChanged;

        private TimeScaleController timeScaleController;
        private ITimeScaleHandle timeScaleHandle;

        private bool isPause = false;
        public bool IsPause
        {
            get => isPause;
            set
            {
                if (isPause != value)
                {
                    isPause = value;
                    timeScaleHandle.TimeScale = value ? 0 : 1;
                    onPauseStateChanged?.Invoke(value);
                }
            }
        }

        public void OnInit(ILevelServiceProvider levelServiceProvider)
        {
            timeScaleController = levelServiceProvider.GetService<TimeScaleController>();
            timeScaleHandle = timeScaleController.GetTimeScaleHandle();
        }
    }
}
