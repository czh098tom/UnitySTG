using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using UnitySTG.Abstractions;

namespace UnitySTG.THSTG.Bullet
{
    [Serializable]
    public class SOBulletStyleSelector : IBulletStyleSubState
    {
        [SerializeField] private bool _useIndependentStyle = false;
        [SerializeField] private BulletStyleAnimatorState _animatorInfo;
        [SerializeField] private BulletStyleOverrideState _independentStyleInfo;

        public int? GetDuration()
        {
            return _useIndependentStyle ? _independentStyleInfo.GetDuration() : _animatorInfo.GetDuration();
        }

        public IDisposable OnIntoState(ILevelServiceProvider levelServiceProvider, Bullet controller)
        {
            return _useIndependentStyle ?
                _independentStyleInfo.OnIntoState(levelServiceProvider, controller) :
                _animatorInfo.OnIntoState(levelServiceProvider, controller);
        }
    }
}
