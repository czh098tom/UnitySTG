using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using UnitySTG.Abstractions;

namespace UnitySTG.THSTG.Bullet
{
    [Serializable]
    public class BulletStyleAnimatorState : IBulletStyleSubState, IDeserializationCallback
    {
        [SerializeField] private string _triggerName;
        [SerializeField] private bool _keepForDuration;
        [SerializeField] private int _durationFrames;

        private int _triggerHash;

        public void OnDeserialization(object sender)
        {
            _triggerHash = Animator.StringToHash(_triggerName);
        }

        public IDisposable OnIntoState(ILevelServiceProvider levelServiceProvider, BulletController controller)
        {
            controller.SetAnimatorVariable(_triggerHash);
            return null;
        }

        public int? GetDuration()
        {
            return _keepForDuration ? _durationFrames : null;
        }
    }
}
