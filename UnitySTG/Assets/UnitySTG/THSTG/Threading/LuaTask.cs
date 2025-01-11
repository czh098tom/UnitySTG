using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using UnitySTG.Abstractions;

namespace UnitySTG.THSTG.Threading
{
    [AsyncMethodBuilder(typeof(LuaTaskAsyncMethodBuilder))]
    public struct LuaTask
    {
        public static LuaTask Delay(int delay, CancellationToken cancellationToken = default)
        {
            return new LuaTask(LuaTaskScheduler.Current.CreateDelay(delay, cancellationToken));
        }

        private readonly ILuaTaskAwaiter _awaiter;

        public LuaTask(ILuaTaskAwaiter awaiter)
        {
            _awaiter = awaiter;
        }

        public readonly ILuaTaskAwaiter GetAwaiter()
        {
            return _awaiter;
        }

        internal readonly void TrySetResult()
        {
            _awaiter.TrySetResult();
        }

        internal readonly void TrySetException(Exception e)
        {
            _awaiter.TrySetException(e);
        }
    }
}
