using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.Abstractions
{
    public class StageInitializationCallback : IStageInitializationCallback
    {
        public static readonly IStageInitializationCallback Empty = new StageInitializationCallback(_ => { });

        private class AppendCallback : IStageInitializationCallback
        {
            private readonly IStageInitializationCallback _inner;
            private readonly Action<ILevelServiceProvider> _action;

            public AppendCallback(IStageInitializationCallback inner, Action<ILevelServiceProvider> action)
            {
                this._inner = inner;
                this._action = action;
            }

            public void OnInit(ILevelServiceProvider levelServiceProvider)
            {
                _inner.OnInit(levelServiceProvider);
                _action?.Invoke(levelServiceProvider);
            }
        }

        private class PrependCallback : IStageInitializationCallback
        {
            private readonly IStageInitializationCallback _inner;
            private readonly Action<ILevelServiceProvider> _action;

            public PrependCallback(IStageInitializationCallback inner, Action<ILevelServiceProvider> action)
            {
                this._inner = inner;
                this._action = action;
            }

            public void OnInit(ILevelServiceProvider levelServiceProvider)
            {
                _action?.Invoke(levelServiceProvider);
                _inner.OnInit(levelServiceProvider);
            }
        }

        public static IStageInitializationCallback Prepend(IStageInitializationCallback inner, Action<ILevelServiceProvider> action)
        {
            return new AppendCallback(inner, action);
        }

        public static IStageInitializationCallback Append(IStageInitializationCallback inner, Action<ILevelServiceProvider> action)
        {
            return new AppendCallback(inner, action);
        }

        public static IStageInitializationCallback Create(Action<ILevelServiceProvider> action)
        {
            return new StageInitializationCallback(action);
        }

        private readonly Action<ILevelServiceProvider> _action;

        private StageInitializationCallback(Action<ILevelServiceProvider> action)
        {
            _action = action;
        }

        public void OnInit(ILevelServiceProvider levelServiceProvider)
        {
            _action?.Invoke(levelServiceProvider);
        }
    }
}
