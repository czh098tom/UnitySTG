using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using UnitySTG.Abstractions;
using UnitySTG.Abstractions.Style;

namespace UnitySTG.THSTG.Bullet
{
    [Serializable]
    public class BulletStyleOverrideState : IBulletStyleSubState
    {
        private class Disposable : IDisposable
        {
            private class DummyObjectFixed : LuaSTGObject
            {
                private readonly int? _keepForDuration;

                private int _timer;

                public DummyObjectFixed(ILevelServiceProvider levelServiceProvider, IObjectStyle style, int? keepForDuration) : base(levelServiceProvider)
                {
                    Style = style;
                    _keepForDuration = keepForDuration;
                }

                protected override void OnFrame()
                {
                    base.OnFrame();
                    if (_keepForDuration.HasValue)
                    {
                        if (_timer >= _keepForDuration)
                        {
                            Dispose();
                        }
                    }
                    _timer++;
                }
            }

            private class DummyObjectFollow : LuaSTGObject
            {
                private readonly BulletController _controller;

                public DummyObjectFollow(ILevelServiceProvider levelServiceProvider, BulletController controller, IObjectStyle style) : base(levelServiceProvider)
                {
                    Style = style;
                    _controller = controller;
                }

                protected override void OnFrame()
                {
                    base.OnFrame();
                    if (_controller.LuaSTGObject.IsValid())
                    {
                        X = _controller.LuaSTGObject.X;
                        Y = _controller.LuaSTGObject.Y;
                    }
                    else
                    {
                        Dispose();
                    }
                }
            }

            private readonly LuaSTGObject _luaSTGObject;

            public Disposable(ILevelServiceProvider levelServiceProvider, BulletController controller, BulletStyleOverrideState state)
            {
                if (!state._syncOriginalMovement)
                {
                    _luaSTGObject = new DummyObjectFixed(levelServiceProvider, state._overrideStyle, state._keepForDuration ? state._durationFrames : null);
                }
                else
                {
                    _luaSTGObject = new DummyObjectFollow(levelServiceProvider, controller, state._overrideStyle);
                }
            }

            public void Dispose()
            {
                _luaSTGObject?.Dispose();
            }
        }

        [SerializeField] private SOObjectStyle _overrideStyle;
        [SerializeField] private bool _syncOriginalMovement;
        [SerializeField] private bool _keepForDuration;
        [SerializeField] private int _durationFrames;

        public IDisposable OnIntoState(ILevelServiceProvider levelServiceProvider, BulletController controller)
        {
            return new Disposable(levelServiceProvider, controller, this);
        }

        public int? GetDuration()
        {
            return _keepForDuration ? _durationFrames : null;
        }
    }
}
