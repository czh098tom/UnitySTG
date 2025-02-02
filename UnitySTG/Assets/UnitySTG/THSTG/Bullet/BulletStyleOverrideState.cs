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
                private readonly Bullet _bullet;

                public DummyObjectFollow(ILevelServiceProvider levelServiceProvider, Bullet bullet, IObjectStyle style) : base(levelServiceProvider)
                {
                    Style = style;
                    _bullet = bullet;
                }

                protected override void OnFrame()
                {
                    base.OnFrame();
                    if (_bullet.IsValid())
                    {
                        X = _bullet.X;
                        Y = _bullet.Y;
                    }
                    else
                    {
                        Dispose();
                    }
                }
            }

            private readonly LuaSTGObject _luaSTGObject;

            public Disposable(ILevelServiceProvider levelServiceProvider, Bullet bullet, BulletStyleOverrideState state)
            {
                if (!state._syncOriginalMovement)
                {
                    _luaSTGObject = new DummyObjectFixed(levelServiceProvider, state._overrideStyle, state._keepForDuration ? state._durationFrames : null);
                }
                else
                {
                    _luaSTGObject = new DummyObjectFollow(levelServiceProvider, bullet, state._overrideStyle);
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

        public IDisposable OnIntoState(ILevelServiceProvider levelServiceProvider, Bullet bullet)
        {
            return new Disposable(levelServiceProvider, bullet, this);
        }

        public int? GetDuration()
        {
            return _keepForDuration ? _durationFrames : null;
        }
    }
}
