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

                public DummyObjectFixed(ILevelServiceProvider levelServiceProvider, BulletObject bullet, IObjectStyle style, int? keepForDuration) : base(levelServiceProvider)
                {
                    Style = style;
                    X = bullet.X;
                    Y = bullet.Y;
                    Rot = bullet.Rot;
                    _keepForDuration = keepForDuration;
                }

                protected override void OnFrame()
                {
                    base.OnFrame();
                    if (_keepForDuration.HasValue)
                    {
                        if (_timer >= _keepForDuration)
                        {
                            Destroy(ManualDelEventArgs.Instance);
                        }
                    }
                    _timer++;
                }
            }

            private class DummyObjectFollow : LuaSTGObject
            {
                private readonly BulletObject _bullet;

                public DummyObjectFollow(ILevelServiceProvider levelServiceProvider, BulletObject bullet, IObjectStyle style) : base(levelServiceProvider)
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
                        Destroy(ManualDelEventArgs.Instance);
                    }
                }
            }

            private readonly LuaSTGObject _luaSTGObject;

            public Disposable(ILevelServiceProvider levelServiceProvider, BulletObject bullet, BulletStyleOverrideState state)
            {
                if (!state._syncOriginalMovement)
                {
                    _luaSTGObject = new DummyObjectFixed(levelServiceProvider, bullet, state._overrideStyle, state._keepForDuration ? state._durationFrames : null);
                }
                else
                {
                    _luaSTGObject = new DummyObjectFollow(levelServiceProvider, bullet, state._overrideStyle);
                }
            }

            public void Dispose()
            {
                _luaSTGObject?.Destroy(ManualDelEventArgs.Instance);
            }
        }

        [SerializeField] private SOObjectStyle _overrideStyle;
        [SerializeField] private bool _syncOriginalMovement;
        [SerializeField] private bool _keepForDuration;
        [SerializeField] private int _durationFrames;

        public IDisposable OnIntoState(ILevelServiceProvider levelServiceProvider, BulletObject bullet)
        {
            return new Disposable(levelServiceProvider, bullet, this);
        }

        public int? GetDuration()
        {
            return _keepForDuration ? _durationFrames : null;
        }
    }
}
