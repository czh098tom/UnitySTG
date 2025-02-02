using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnitySTG.Abstractions;
using UnitySTG.Abstractions.Extension;

namespace UnitySTG.THSTG.Stage.Extension
{
    public static class StageFrameExtension
    {
        public static IStageFrameCallback AttachDefault(this IStageFrameCallback callback)
        {
            return callback.Append(s =>
            {
                s.Pool.UpdateXY();
                s.Pool.UpdateRot();
                s.Pool.DoFrame();
                s.Pool.CollisionCheck();
                s.Pool.CheckBounds();
                s.Pool.PerformKill();
                s.Pool.TryCompressObjectID();
            });
        }
    }
}
