using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnitySTG.Abstractions
{
    public class LevelController : MonoBehaviour, ILevelServiceProvider
    {
        [SerializeField] private GameObjectPool _pool;

        private IStageFrameCallback _stage;

        public GameObjectPool Pool => _pool;

        public IStageFinishCallback LifeCycle { get; internal set; }

        public object GetService(Type serviceType)
        {
            return GetComponent(serviceType);
        }

        public void SetStage(IStageFrameCallback stageFrameCallback)
        {
            _stage = stageFrameCallback;
        }

        private void FixedUpdate()
        {
            _stage?.OnFrame(this);
        }
    }
}
