using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.Abstractions.ObjectPoolExtension
{
    public static class StageExtension
    {
        public static IStage ThenUpdateXY(this IStage inner, ILevelServiceProvider levelServiceProvider)
        {
            return Stage.CreateThen(inner, () => levelServiceProvider.Pool.UpdateXY());
        }

        public static IStage ThenUpdateRot(this IStage inner, ILevelServiceProvider levelServiceProvider)
        {
            return Stage.CreateThen(inner, () => levelServiceProvider.Pool.UpdateRot());
        }

        public static IStage ThenDoFrame(this IStage inner, ILevelServiceProvider levelServiceProvider)
        {
            return Stage.CreateThen(inner, () => levelServiceProvider.Pool.DoFrame());
        }

        public static IStage ThenDoCollisionCheck(this IStage inner, ILevelServiceProvider levelServiceProvider)
        {
            return Stage.CreateThen(inner, () => levelServiceProvider.Pool.CollisionCheck());
        }

        public static IStage ThenCheckBounds(this IStage inner, ILevelServiceProvider levelServiceProvider)
        {
            return Stage.CreateThen(inner, () => levelServiceProvider.Pool.CheckBounds());
        }

        public static IStage ThenPerformKill(this IStage inner, ILevelServiceProvider levelServiceProvider)
        {
            return Stage.CreateThen(inner, () => levelServiceProvider.Pool.PerformKill());
        }

        public static IStage ThenTryCompressLayerID(this IStage inner, ILevelServiceProvider levelServiceProvider)
        {
            return Stage.CreateThen(inner, () => levelServiceProvider.Pool.TryCompressObjectID());
        }
    }
}
