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
        [SerializeField] private UnityEvent<bool> _onPauseStateChanged;

        private TimeScaleController _timeScaleController;
        private ITimeScaleHandle _timeScaleHandle;

        private bool _isPause = false;
        public bool IsPause
        {
            get => _isPause;
            set
            {
                if (_isPause != value)
                {
                    _isPause = value;
                    _timeScaleHandle.TimeScale = value ? 0 : 1;
                    _onPauseStateChanged?.Invoke(value);
                }
            }
        }

        public void OnInit(ILevelServiceProvider levelServiceProvider)
        {
            _timeScaleController = levelServiceProvider.GetService<TimeScaleController>();
            _timeScaleHandle = _timeScaleController.GetTimeScaleHandle();
        }
    }
}
