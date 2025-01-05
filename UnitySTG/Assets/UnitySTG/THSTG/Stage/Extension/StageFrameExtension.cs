using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnitySTG.Abstractions;
using UnitySTG.Abstractions.ObjectPoolExtension;

namespace UnitySTG.THSTG.Stage.Extension
{
    public static class StageFrameExtension
    {
        public static IStageFrameCallback AttachDefault(this IStageFrameCallback callback)
        {
            return callback
                .AppendUpdateXY()
                .AppendUpdateRot()
                .AppendDoFrame()
                .AppendDoCollisionCheck()
                .AppendCheckBounds()
                .AppendPerformKill()
                .AppendTryCompressLayerID();
        }
    }
}
