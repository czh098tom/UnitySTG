using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using UnitySTG.Abstractions.ObjectPoolExtension;

namespace UnitySTG.Abstractions
{
    public class LevelController : MonoBehaviour
    {
        private IStage _stage;

        public void SetStage(Action frame)
        {
            _stage = Stage.Create(frame)
                .ThenUpdateXY()
                .ThenUpdateRot()
                .ThenDoFrame()
                .ThenDoCollisionCheck()
                .ThenCheckBounds()
                .ThenPerformKill()
                .ThenTryCompressLayerID();
        }

        private void FixedUpdate()
        {
            _stage?.OnFrame();
        }
    }
}
