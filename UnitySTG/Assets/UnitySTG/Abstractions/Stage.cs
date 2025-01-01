using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.Abstractions
{
    public class Stage : IStageFrameCallback
    {
        private class ThenStage : IStageFrameCallback
        {
            private readonly IStageFrameCallback _inner;
            private readonly Action<ILevelServiceProvider> _action;

            public ThenStage(IStageFrameCallback inner, Action<ILevelServiceProvider> action)
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

        public static IStageFrameCallback CreateThen(IStageFrameCallback inner, Action<ILevelServiceProvider> action)
        {
            return new ThenStage(inner, action);
        }

        public static IStageFrameCallback Create(Action<ILevelServiceProvider> action)
        {
            return new Stage(action);
        }

        private readonly Action<ILevelServiceProvider> _action;

        private Stage(Action<ILevelServiceProvider> action)
        {
            _action = action;
        }

        public void OnFrame(ILevelServiceProvider levelServiceProvider)
        {
            _action?.Invoke(levelServiceProvider);
        }
    }
}
