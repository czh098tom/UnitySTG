﻿using System;
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
        [SerializeField] private GameObject _defaultStyleTemplate;
        [SerializeField] private GameObject _defaultStyleTemplateInstance;

        private ILevelServiceProvider _levelServiceProvider;
        private Dictionary<GameObject, GameObject> _cachedStyleTemplate;

        private GameObject _currentStyleTemplate;

        internal LuaSTGObject _luaSTGObject;
        private int _objectID;

        internal GameObjectState State { get; private set; }
        internal GameObjectController CollisionNext { get; set; }
        internal GameObjectController CollisionPrev { get; set; }
        internal bool InCollisionList { get; set; }
        internal GameObjectController UpdateNext { get; set; }
        internal GameObjectController UpdatePrev { get; set; }
        internal bool InUpdateList { get; set; }

        internal Animator Animator => _animator;

        internal int ObjectID
        {
            get => _objectID;
            set
            {
                _objectID = value;
                if (_renderer != null)
                {
                    _renderer.rendererPriority = value;
                }
            }
        }

        private void SetRenderer(Renderer value)
        {
            _renderer = value;
            if (_renderer != null)
            {
                _renderer.rendererPriority = _objectID;
                _renderer.sortingOrder = _layer;
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
                _levelServiceProvider.Pool.RemoveFromCollisionCheckLinkedList(this);
                _group = value;
                _levelServiceProvider.Pool.AddToCollisionCheckLinkedList(this);
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
                        _levelServiceProvider.Pool.AddToCollisionCheckLinkedList(this);
                    }
                    else
                    {
                        _levelServiceProvider.Pool.RemoveFromCollisionCheckLinkedList(this);
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

        private ColliderShape _colliderShape;
        internal ColliderShape ColliderShape
        {
            get => _colliderShape;
            set => _colliderShape = value;
        }

        private void ThrowIfInCollisionCheck()
        {
            if (_levelServiceProvider.Pool.State == GameObjectPool.CallbackState.Collision)
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

        #region style
        private IObjectStyle _style;
        internal IObjectStyle Style
        {
            get => _style;
            set
            {
                if (_style != value)
                {
                    ChangeStyle(value, _style);
                    _style = value;
                }
            }
        }

        private void ChangeStyle(IObjectStyle @new, IObjectStyle original)
        {
            if (_currentStyleTemplate != null)
            {
                original?.ResetTemplate(_currentStyleTemplate, _renderer, _animator);
                _currentStyleTemplate.SetActive(false);
            }
            if (@new != null)
            {
                var newTemplate = @new.GetTemplate();
                if (original == null || newTemplate != original.GetTemplate())
                {
                    if (!_cachedStyleTemplate.TryGetValue(newTemplate, out var instance))
                    {
                        instance = Instantiate(newTemplate, transform);
                        _cachedStyleTemplate.Add(newTemplate, instance);
                    }
                    _currentStyleTemplate = instance;
                    _currentStyleTemplate.SetActive(true);
                }
                _animator = @new.UpdateAnimator(_currentStyleTemplate);
                SetRenderer(@new.UpdateRenderer(_currentStyleTemplate));
            }
            else
            {
                _animator = null;
                _renderer = null;
                _currentStyleTemplate = null;
            }
        }
        #endregion

        private int _layer;
        internal int Layer
        {
            get => _layer;
            set
            {
                _layer = value;
                if (_renderer != null)
                {
                    _renderer.sortingOrder = value;
                }
            }
        }

        private void Awake()
        {
            _cachedStyleTemplate = new()
            {
                [_defaultStyleTemplate] = _defaultStyleTemplateInstance
            };
        }

        public void OnCreated(ILevelServiceProvider levelServiceProvider, int objectID)
        {
            _levelServiceProvider = levelServiceProvider;
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
            Style = null;
        }

        public void OnFrame()
        {
            _luaSTGObject.OnFrame();
        }

        public void Destroy(DestroyEventArgs args)
        {
            State = GameObjectState.Dying;
            _luaSTGObject.OnDestroy(args);
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

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_levelServiceProvider == null) return;
            var desc = _levelServiceProvider.Pool.ColliderGizmoDescriptor;
            if (desc != null && desc.Info.TryGetValue(_group, out var info))
            {
                var color = Gizmos.color;
                Gizmos.color = new Color(info.Color.r, info.Color.g, info.Color.b, info.Color.a * 0.75f);
                transform.GetPositionAndRotation(out var pos, out var rot);
                var mesh = _colliderShape switch
                {
                    ColliderShape.Circular => GizmoMeshProvider.Instance.Ellipse,
                    _ => GizmoMeshProvider.Instance.Rect,
                };
                //Gizmos.DrawSphere(transform.position, (float)A);
                Gizmos.DrawMesh(mesh, 0, pos, rot, new Vector3((float)A, (float)B, 1));
                //Gizmos.DrawMesh(mesh, 0, Vector3.zero, Quaternion.identity, Vector3.one * 200);
                Gizmos.color = color;
            }
        }
#endif
    }
}
