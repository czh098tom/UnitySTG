using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using UnitySTG.Abstractions.ObjectPoolExtension;

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

        public void SetStage(Action<ILevelServiceProvider> frame)
        {
            _stage = Stage.Create(frame)
                .ThenUpdateXY(this)
                .ThenUpdateRot(this)
                .ThenDoFrame(this)
                .ThenDoCollisionCheck(this)
                .ThenCheckBounds(this)
                .ThenPerformKill(this)
                .ThenTryCompressLayerID(this);
        }

        private void FixedUpdate()
        {
            _stage?.OnFrame(this);
        }
    }
}
