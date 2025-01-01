using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.Abstractions.ObjectPoolExtension
{
    public static class StageExtension
    {
        public static IStageFrameCallback ThenUpdateXY(this IStageFrameCallback inner, ILevelServiceProvider levelServiceProvider)
        {
            return Stage.CreateThen(inner, _ => levelServiceProvider.Pool.UpdateXY());
        }

        public static IStageFrameCallback ThenUpdateRot(this IStageFrameCallback inner, ILevelServiceProvider levelServiceProvider)
        {
            return Stage.CreateThen(inner, _ => levelServiceProvider.Pool.UpdateRot());
        }

        public static IStageFrameCallback ThenDoFrame(this IStageFrameCallback inner, ILevelServiceProvider levelServiceProvider)
        {
            return Stage.CreateThen(inner, _ => levelServiceProvider.Pool.DoFrame());
        }

        public static IStageFrameCallback ThenDoCollisionCheck(this IStageFrameCallback inner, ILevelServiceProvider levelServiceProvider)
        {
            return Stage.CreateThen(inner, _ => levelServiceProvider.Pool.CollisionCheck());
        }

        public static IStageFrameCallback ThenCheckBounds(this IStageFrameCallback inner, ILevelServiceProvider levelServiceProvider)
        {
            return Stage.CreateThen(inner, _ => levelServiceProvider.Pool.CheckBounds());
        }

        public static IStageFrameCallback ThenPerformKill(this IStageFrameCallback inner, ILevelServiceProvider levelServiceProvider)
        {
            return Stage.CreateThen(inner, _ => levelServiceProvider.Pool.PerformKill());
        }

        public static IStageFrameCallback ThenTryCompressLayerID(this IStageFrameCallback inner, ILevelServiceProvider levelServiceProvider)
        {
            return Stage.CreateThen(inner, _ => levelServiceProvider.Pool.TryCompressObjectID());
        }
    }
}
