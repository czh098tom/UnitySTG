using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.Abstractions
{
    public class StageFrameCallback : IStageFrameCallback
    {
        public static readonly IStageFrameCallback Empty = new StageFrameCallback(_ => { });

        private class AppendCallback : IStageFrameCallback
        {
            private readonly IStageFrameCallback _inner;
            private readonly Action<ILevelServiceProvider> _action;

            public AppendCallback(IStageFrameCallback inner, Action<ILevelServiceProvider> action)
            {
                this._inner = inner;
                this._action = action;
            }

            public void OnFrame(ILevelServiceProvider levelServiceProvider)
            {
                _inner.OnFrame(levelServiceProvider);
                _action?.Invoke(levelServiceProvider);
            }
        }

        private class PrependCallback : IStageFrameCallback
        {
            private readonly IStageFrameCallback _inner;
            private readonly Action<ILevelServiceProvider> _action;

            public PrependCallback(IStageFrameCallback inner, Action<ILevelServiceProvider> action)
            {
                this._inner = inner;
                this._action = action;
            }

            public void OnFrame(ILevelServiceProvider levelServiceProvider)
            {
                _action?.Invoke(levelServiceProvider);
                _inner.OnFrame(levelServiceProvider);
            }
        }

        public static IStageFrameCallback Prepend(IStageFrameCallback inner, Action<ILevelServiceProvider> action)
        {
            return new PrependCallback(inner, action);
        }

        public static IStageFrameCallback Append(IStageFrameCallback inner, Action<ILevelServiceProvider> action)
        {
            return new AppendCallback(inner, action);
        }

        public static IStageFrameCallback Create(Action<ILevelServiceProvider> action)
        {
            return new StageFrameCallback(action);
        }

        private readonly Action<ILevelServiceProvider> _action;

        private StageFrameCallback(Action<ILevelServiceProvider> action)
        {
            _action = action;
        }

        public void OnFrame(ILevelServiceProvider levelServiceProvider)
        {
            _action?.Invoke(levelServiceProvider);
        }
    }
}
