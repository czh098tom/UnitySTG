using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.Abstractions.ObjectPoolExtension
{
    public static class StageInitializationExtension
    {
        public static IStageInitializationCallback Prepend(this IStageInitializationCallback callback, Action<ILevelServiceProvider> action)
        {
            return StageInitializationCallback.Prepend(callback, action);
        }

        public static IStageInitializationCallback Append(this IStageInitializationCallback callback, Action<ILevelServiceProvider> action)
        {
            return StageInitializationCallback.Append(callback, action);
        }
    }
}
