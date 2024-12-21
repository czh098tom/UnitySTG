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

        private IStage _stage;

        public GameObjectPool Pool => _pool;

        public void SetStage(Action frame)
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
            _stage?.OnFrame();
        }
    }
}
