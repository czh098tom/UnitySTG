﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics.FixedPoint;
using UnityEngine;

namespace UnitySTG.Abstractions
{
    public class LuaSTGObject : IDisposable
    {
        private readonly GameObjectController _controller;
        public ILevelServiceProvider LevelServiceProvider { get; }

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
        public byte Group
        {
            get
            {
                ThrowIfDead();
                return _controller.Group;
            }
            set
            {
                ThrowIfDead();
                _controller.Group = value;
            }
        }

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

        public ColliderShape ColliderShape
        {
            get
            {
                ThrowIfDead();
                return _controller.ColliderShape;
            }
            set
            {
                ThrowIfDead();
                _controller.ColliderShape = value;
            }
        }
        #endregion

        #region bound
        public bool Bound
        {
            get
            {
                ThrowIfDead();
                return _controller.Bound;
            }
            set
            {
                ThrowIfDead();
                _controller.Bound = value;
            }
        }

        public BoundCheckType BoundType
        {
            get
            {
                ThrowIfDead();
                return _controller.BoundType;
            }
            set
            {
                ThrowIfDead();
                _controller.BoundType = value;
            }
        }
        #endregion

        public int Layer
        {
            get
            {
                ThrowIfDead();
                return _controller.Layer;
            }
            set
            {
                ThrowIfDead();
                _controller.Layer = value;
            }
        }

        public IObjectStyle Style
        {
            get
            {
                ThrowIfDead();
                return _controller.Style;
            }
            set
            {
                ThrowIfDead(); 
                _controller.Style = value;
            }
        }

        public Animator Animator
        {
            get
            {
                ThrowIfDead();
                return _controller.Animator;
            }
        }

        public LuaSTGObject(ILevelServiceProvider levelServiceProvider)
        {
            _controller = levelServiceProvider.Pool.Allocate();
            _controller._luaSTGObject = this;
            LevelServiceProvider = levelServiceProvider;
        }

        internal protected virtual void OnFrame()
        {

        }

        internal protected virtual void OnDestroy(DestroyEventArgs args)
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

        public void Destroy(ManualDelEventArgs eventArgs)
        {
            if (!IsValid()) return;
            Dispose(eventArgs, true);
            Disposed = true;
        }

        public void Dispose()
        {
            Destroy(ManualDelEventArgs.Instance);
        }

        ~LuaSTGObject()
        {
            Dispose(ManualDelEventArgs.Instance, false);
        }

        protected virtual void Dispose(ManualDelEventArgs args, bool disposing)
        {
            if (disposing)
            {
                _controller.Destroy(args);
            }
        }

        private void ThrowIfDead()
        {
            if (!IsValid()) throw new InvalidOperationException("Invalid lstg object.");
        }
    }
}
