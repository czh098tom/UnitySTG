using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.Abstractions.Extension
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
    }
}
