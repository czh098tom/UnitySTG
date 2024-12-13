using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Unity.Mathematics.FixedPoint;

namespace UnitySTG.Abstractions
{
    internal class GameObjectController : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Animator _animator;

        internal LuaSTGObject _luaSTGObject;
        private int _objectID;

        internal GameObjectState State { get; private set; }
        internal GameObjectController CollisionNext { get; set; }
        internal GameObjectController CollisionPrev { get; set; }
        internal GameObjectController UpdateNext { get; set; }
        internal GameObjectController UpdatePrev { get; set; }

        internal int ObjectID
        {
            get => _objectID;
            set
            {
                _objectID = value;
                _renderer.rendererPriority = value;
            }
        }

        #region xy
        private fp _x;
        internal fp X
        {
            get => _x;
            set
            {
                _x = value;
                UpdateTransform();
            }
        }

        private fp _y;
        internal fp Y
        {
            get => _y;
            set
            {
                _y = value;
                UpdateTransform();
            }
        }

        internal void UpdateXY()
        {
            _x += _vx;
            _y += _vy;
            UpdateTransform();
        }

        private fp _vx;
        internal fp VX
        {
            get => _vx;
            set => _vx = value;
        }

        private fp _vy;
        internal fp VY
        {
            get => _vy;
            set => _vy = value;
        }

        private void UpdateTransform()
        {
            transform.localPosition = new Vector2((float)_x, (float)_y);
        }
        #endregion

        #region rot
        internal fp _rot;
        internal fp Rot
        {
            get => _rot;
            set
            {
                _rot = value;
                transform.localRotation = Quaternion.Euler(0, 0, (float)_rot);
            }
        }

        internal fp _omega;
        internal fp Omega
        {
            get => _omega;
            set => _omega = value;
        }

        internal void UpdateRot()
        {
            Rot = _rot + _omega;
        }
        #endregion

        #region colli
        private byte _group;
        internal byte Group
        {
            get => _group;
            set
            {
                ThrowIfInCollisionCheck();
                GameObjectPool.Instance.RemoveFromCollisionCheckLinkedList(this);
                _group = value;
                GameObjectPool.Instance.AddToCollisionCheckLinkedList(this);
            }
        }

        private bool _colli = true;
        internal bool Colli
        {
            get => _colli;
            set
            {
                ThrowIfInCollisionCheck();
                if (_colli != value)
                {
                    _colli = value;
                    if (value)
                    {
                        GameObjectPool.Instance.AddToCollisionCheckLinkedList(this);
                    }
                    else
                    {
                        GameObjectPool.Instance.RemoveFromCollisionCheckLinkedList(this);
                    }
                }
            }
        }

        private fp _a;
        internal fp A
        {
            get => _a;
            set => _a = value;
        }

        private fp _b;
        internal fp B
        {
            get => _b;
            set => _b = value;
        }

        private ColliderShape _shape;
        internal ColliderShape Shape
        {
            get => _shape;
            set => _shape = value;
        }

        private static void ThrowIfInCollisionCheck()
        {
            if (GameObjectPool.Instance.State == GameObjectPool.CallbackState.Collision)
            {
                throw new InvalidOperationException("Group cannot be modified in collision check");
            }
        }
        #endregion

        #region bound

        private bool _bound = true;
        internal bool Bound
        {
            get => _bound;
            set => _bound = value;
        }

        private BoundCheckType _boundType = BoundCheckType.XY;
        internal BoundCheckType BoundType
        {
            get => _boundType;
            set => _boundType = value;
        }
        #endregion

        public void OnCreated(int objectID)
        {
            State = GameObjectState.Alive;
            ObjectID = objectID;
            _x = 0;
            _y = 0;
            UpdateTransform();
            _vx = 0;
            _vy = 0;
            _rot = 0;
            _omega = 0;
            _group = 0;
            _colli = true;
            _a = 0;
            _b = 0;
            _bound = true;
            _boundType = BoundCheckType.XY;
        }

        public void OnFrame()
        {
            _luaSTGObject.OnFrame();
        }

        public void Del()
        {
            State = GameObjectState.Dying;
            _luaSTGObject.OnDel();
        }

        public void OnReturned()
        {
            State = GameObjectState.Dead;
        }

        public void OnColli(GameObjectController other)
        {
            _luaSTGObject.OnColli(other._luaSTGObject);
        }

        public void SetV2(fp speed, fp angle, bool setRotation = true)
        {
            var rad = fpmath.radians(angle);
            var vx = speed * fpmath.cos(rad);
            var vy = speed * fpmath.sin(rad);
            _vx = vx;
            _vy = vy;
            if (setRotation)
            {
                Rot = angle;
            }
        }
    }
}
