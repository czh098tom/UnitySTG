using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.Abstractions.ObjectPoolExtension
{
    public static class StageExtension
    {
        public static IStage ThenUpdateXY(this IStage inner)
        {
            return Stage.CreateThen(inner, () => GameObjectPool.Instance.UpdateXY());
        }

        public static IStage ThenUpdateRot(this IStage inner)
        {
            return Stage.CreateThen(inner, () => GameObjectPool.Instance.UpdateRot());
        }

        public static IStage ThenDoFrame(this IStage inner)
        {
            return Stage.CreateThen(inner, () => GameObjectPool.Instance.DoFrame());
        }

        public static IStage ThenDoCollisionCheck(this IStage inner)
        {
            return Stage.CreateThen(inner, () => GameObjectPool.Instance.CollisionCheck());
        }

        public static IStage ThenCheckBounds(this IStage inner)
        {
            return Stage.CreateThen(inner, () => GameObjectPool.Instance.CheckBounds());
        }

        public static IStage ThenPerformKill(this IStage inner)
        {
            return Stage.CreateThen(inner, () => GameObjectPool.Instance.PerformKill());
        }

        public static IStage ThenTryCompressLayerID(this IStage inner)
        {
            return Stage.CreateThen(inner, () => GameObjectPool.Instance.TryCompressObjectID());
        }
    }
}
