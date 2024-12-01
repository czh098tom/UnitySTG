using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.Abstractions
{
    public class Stage : IStage
    {
        private class ThenStage : IStage
        {
            private readonly IStage _inner;
            private readonly Action _action;

            public ThenStage(IStage inner, Action action)
            {
                this._inner = inner;
                this._action = action;
            }

            public void OnFrame()
            {
                _inner.OnFrame();
                _action?.Invoke();
            }
        }

        public static IStage CreateThen(IStage inner, Action action)
        {
            return new ThenStage(inner, action);
        }

        public static IStage Create(Action action)
        {
            return new Stage(action);
        }

        private readonly Action _action;

        private Stage(Action action)
        {
            _action = action;
        }

        public void OnFrame()
        {
            _action?.Invoke();
        }
    }
}
