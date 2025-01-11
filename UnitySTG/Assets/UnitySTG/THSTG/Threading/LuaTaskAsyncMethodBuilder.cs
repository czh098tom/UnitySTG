using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnitySTG.THSTG.Threading
{
    public readonly struct LuaTaskAsyncMethodBuilder
    {
        private readonly LuaTask _luaTask;

        public readonly LuaTask Task => _luaTask;

        private LuaTaskAsyncMethodBuilder(LuaTask luaTask)
        {
            _luaTask = luaTask;
        }

        public static LuaTaskAsyncMethodBuilder Create()
        {
            return new(new(LuaTaskScheduler.Current.CreateAsyncMethod()));
        }

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            awaiter.OnCompleted(stateMachine.MoveNext);
        }

        [SecuritySafeCritical]
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter,
            ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            awaiter.UnsafeOnCompleted(stateMachine.MoveNext);
        }

        public void SetResult()
        {
            _luaTask.TrySetResult();
        }

        public void SetException(Exception exception)
        {
            _luaTask.TrySetException(exception);
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }
}
