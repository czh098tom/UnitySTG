using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.Abstractions.ObjectPoolExtension
{
    public static class StageFrameExtension
    {
        public static IStageFrameCallback Prepend(this IStageFrameCallback callback, Action<ILevelServiceProvider> action)
        {
            return StageFrameCallback.Prepend(callback, action);
        }

        public static IStageFrameCallback Append(this IStageFrameCallback callback, Action<ILevelServiceProvider> action) 
        {
            return StageFrameCallback.Append(callback, action);
        }

        public static IStageFrameCallback AppendUpdateXY(this IStageFrameCallback inner)
        {
            return StageFrameCallback.Append(inner, provider => provider.Pool.UpdateXY());
        }

        public static IStageFrameCallback AppendUpdateRot(this IStageFrameCallback inner)
        {
            return StageFrameCallback.Append(inner, provider => provider.Pool.UpdateRot());
        }

        public static IStageFrameCallback AppendDoFrame(this IStageFrameCallback inner)
        {
            return StageFrameCallback.Append(inner, provider => provider.Pool.DoFrame());
        }

        public static IStageFrameCallback AppendDoCollisionCheck(this IStageFrameCallback inner)
        {
            return StageFrameCallback.Append(inner, provider => provider.Pool.CollisionCheck());
        }

        public static IStageFrameCallback AppendCheckBounds(this IStageFrameCallback inner)
        {
            return StageFrameCallback.Append(inner, provider => provider.Pool.CheckBounds());
        }

        public static IStageFrameCallback AppendPerformKill(this IStageFrameCallback inner)
        {
            return StageFrameCallback.Append(inner, provider => provider.Pool.PerformKill());
        }

        public static IStageFrameCallback AppendTryCompressLayerID(this IStageFrameCallback inner)
        {
            return StageFrameCallback.Append(inner, provider => provider.Pool.TryCompressObjectID());
        }
    }
}
