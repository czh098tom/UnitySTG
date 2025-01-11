using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnitySTG.Abstractions;

namespace UnitySTG.THSTG.Threading
{
    public class LuaTaskScheduler : MonoBehaviour, IStageFrameCallback, IStageInitializationCallback
    {
        private static LuaTaskScheduler _current;
        internal static LuaTaskScheduler Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (_current == null) throw new InvalidOperationException();
                return _current;
            }
        }

        private long _time;

        private readonly LinkedList<ILuaTaskAwaiter> _awaiters = new();

        public void OnInit(ILevelServiceProvider levelServiceProvider)
        {
            _current = this;
        }

        public void OnFrame(ILevelServiceProvider levelServiceProvider)
        {
            var node = _awaiters.First;
            while (node != null)
            {
                if (node.Value.IsCompleted || node.Value.IsFaulted)
                {
                    var curr = node;
                    node = node.Next;
                    curr.Value.TrySetResult();
                }
                else
                {
                    node = node.Next;
                }
            }
            _time++;
        }

        private void OnDestroy()
        {
            _current = null;
        }

        public ILuaTaskAwaiter CreateDelay(int delay, CancellationToken cancellationToken = default)
            => new DelayAwaiter(this, delay, cancellationToken);

        public ILuaTaskAwaiter CreateAsyncMethod(CancellationToken cancellationToken = default)
            => new Awaiter(this, cancellationToken);

        private abstract class AwaiterBase : ILuaTaskAwaiter, IDisposable
        {
            protected readonly LuaTaskScheduler _luaTaskScheduler;
            private readonly LinkedListNode<ILuaTaskAwaiter> _node;
            private readonly CancellationToken _cancellationToken;
            private bool _disposed = false;

            private Exception _exception;
            private Action _continuation;

            public abstract bool IsCompleted { get; }
            public bool IsFaulted => _cancellationToken.IsCancellationRequested || _exception != null;

            public AwaiterBase(LuaTaskScheduler luaTaskScheduler, CancellationToken cancellationToken)
            {
                _node = new LinkedListNode<ILuaTaskAwaiter>(this);
                luaTaskScheduler._awaiters.AddLast(_node);
                _luaTaskScheduler = luaTaskScheduler;
                _cancellationToken = cancellationToken;
            }

            public void GetResult()
            {
                if (_cancellationToken.IsCancellationRequested)
                {
                    throw new OperationCanceledException(_cancellationToken);
                }
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            public void OnCompleted(Action continuation)
            {
                _continuation = continuation;
                if (IsCompleted)
                {
                    TrySetResult();
                }
            }

            public void Dispose()
            {
                if (_disposed) return;
                _luaTaskScheduler._awaiters.Remove(_node);
                _disposed = true;
            }

            public virtual void TrySetResult()
            {
                Dispose();
                _continuation?.Invoke();
                _continuation = null;
            }

            public virtual void TrySetException(Exception e)
            {
                _exception = e;
                Dispose();
                _continuation?.Invoke();
                _continuation = null;
            }
        }

        private class DelayAwaiter : AwaiterBase
        {
            private readonly long _due;

            public override bool IsCompleted => _luaTaskScheduler._time >= _due;

            public DelayAwaiter(LuaTaskScheduler luaTaskScheduler, int delay, CancellationToken cancellationToken)
                : base(luaTaskScheduler, cancellationToken)
            {
                _due = _luaTaskScheduler._time + delay;
            }
        }

        private class Awaiter : AwaiterBase
        {
            private bool _isCompleted;
            public override bool IsCompleted => _isCompleted;

            public Awaiter(LuaTaskScheduler luaTaskScheduler, CancellationToken cancellationToken)
                : base(luaTaskScheduler, cancellationToken)
            {

            }

            public override void TrySetResult()
            {
                _isCompleted = true;
                base.TrySetResult();
            }

            public override void TrySetException(Exception e)
            {
                _isCompleted = true;
                base.TrySetException(e);
            }
        }
    }
}
