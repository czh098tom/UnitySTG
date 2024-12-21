using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics.FixedPoint;

namespace UnitySTG.Abstractions
{
    public class LuaSTGObject : IDisposable
    {
        private readonly GameObjectController _controller;
        public bool Disposed { get; private set; } = false;

        #region xy
        public fp X
        {
            get
            {
                ThrowIfDead();
                return _controller.X;
            }
            set
            {
                ThrowIfDead();
                _controller.X = value;
            }
        }

        public fp Y
        {
            get
            {
                ThrowIfDead();
                return _controller.Y;
            }
            set
            {
                ThrowIfDead();
                _controller.Y = value;
            }
        }

        public fp VX
        {
            get
            {
                ThrowIfDead();
                return _controller.VX;
            }
            set
            {
                ThrowIfDead();
                _controller.VX = value;
            }
        }

        public fp VY
        {
            get
            {
                ThrowIfDead();
                return _controller.VY;
            }
            set
            {
                ThrowIfDead();
                _controller.VY = value;
            }
        }
        #endregion

        #region rot
        public fp Rot
        {
            get
            {
                ThrowIfDead();
                return _controller.Rot;
            }
            set
            {
                ThrowIfDead();
                _controller.Rot = value;
            }
        }

        public fp Omega
        {
            get
            {
                ThrowIfDead();
                return _controller.Omega;
            }
            set
            {
                ThrowIfDead();
                _controller.Omega = value;
            }
        }
        #endregion

        #region colli
        public fp A
        {
            get
            {
                ThrowIfDead();
                return _controller.A;
            }
            set
            {
                ThrowIfDead();
                _controller.A = value;
            }
        }

        public fp B
        {
            get
            {
                ThrowIfDead();
                return _controller.B;
            }
            set
            {
                ThrowIfDead();
                _controller.B = value;
            }
        }

        public bool Colli
        {
            get
            {
                ThrowIfDead();
                return _controller.Colli;
            }
            set
            {
                ThrowIfDead();
                _controller.Colli = value;
            }
        }

        public ColliderShape Shape
        {
            get
            {
                ThrowIfDead();
                return _controller.Shape;
            }
            set
            {
                ThrowIfDead();
                _controller.Shape = value;
            }
        }
        #endregion

        public IObjectStyle Style
        {
            get => _controller.Style;
            set => _controller.Style = value;
        }

        public LuaSTGObject(ILevelServiceProvider levelServiceProvider)
        {
            _controller = levelServiceProvider.Pool.Allocate();
            _controller._luaSTGObject = this;
        }

        internal protected virtual void OnFrame()
        {

        }

        internal protected virtual void OnDel()
        {

        }

        internal protected virtual void OnColli(LuaSTGObject other)
        {

        }

        public bool IsValid()
        {
            return !Disposed && _controller.State != GameObjectState.Dead;
        }

        public void SetV2(fp speed, fp angle, bool setRotation = true)
        {
            ThrowIfDead();
            _controller.SetV2(speed, angle, setRotation);
        }

        public void Dispose()
        {
            if (!IsValid()) return;
            _controller.Del();
            Disposed = true;
        }

        private void ThrowIfDead()
        {
            if (!IsValid()) throw new InvalidOperationException("Invalid lstg object.");
        }
    }
}
