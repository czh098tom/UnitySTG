using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

using Latticework.UnityEngine.Utilities;
using Unity.Mathematics.FixedPoint;

namespace UnitySTG.Abstractions
{
    public class GameObjectPool : Singleton<GameObjectPool>
    {
        internal static readonly byte COLLISION_GROUP_MAX = 32;

        internal enum CallbackState
        {
            Idle,
            Collision,
        }

        [SerializeField] private GameObjectController _prefab;
        [SerializeField] private int _maxSize = 65536;

        internal CallbackState State { get; private set; }

        private long _currObjID = 0L;

        private fp4 _bounds = new fp4(-224, -256, 224, 256);

        private readonly CollisionChecker _collisionChecker = new();

        private ObjectPool<GameObjectController> _pool;
        private readonly GameObjectController[] _collisionCheckLinkedList = new GameObjectController[COLLISION_GROUP_MAX];
        private GameObjectController _updateHead;

        private uint[] _collisionMasks = new uint[COLLISION_GROUP_MAX];

        protected override void Awake()
        {
            base.Awake();
            _pool = new(() =>
            {
                return Instantiate(_prefab, transform);
            }, actionOnGet: ctrl =>
            {
                _currObjID++;
                ctrl.gameObject.SetActive(true);
                ctrl.OnCreated(_currObjID);
                AddToCollisionCheckLinkedList(ctrl);
                AddToUpdateLinkedList(ctrl);
            }, actionOnRelease: ctrl =>
            {
                RemoveFromUpdateLinkedList(ctrl);
                RemoveFromCollisionCheckLinkedList(ctrl);
                ctrl.gameObject.SetActive(false);
                ctrl.OnReturned();
            }, defaultCapacity: 1000, maxSize: _maxSize);
        }

        internal GameObjectController Allocate()
        {
            return _pool.Get();
        }

        internal void Return(GameObjectController gameObjectController)
        {
            _pool.Release(gameObjectController);
        }

        internal void AddToCollisionCheckLinkedList(GameObjectController ctrl)
        {
            var originalHead = _collisionCheckLinkedList[0];
            ctrl.CollisionNext = originalHead;
            ctrl.CollisionPrev = null;
            if (originalHead != null)
            {
                originalHead.CollisionPrev = ctrl;
            }
            _collisionCheckLinkedList[0] = ctrl;
        }

        internal void RemoveFromCollisionCheckLinkedList(GameObjectController ctrl)
        {
            var originalNext = ctrl.CollisionNext;
            var originalPrev = ctrl.CollisionPrev;
            if (originalNext != null)
            {
                originalNext.CollisionPrev = originalPrev;
                if (originalPrev == null)
                {
                    _collisionCheckLinkedList[ctrl.Group] = originalNext;
                }
            }
            if (originalPrev != null)
            {
                originalPrev.CollisionNext = originalNext;
            }
            else
            {
                if (originalNext != null)
                {
                    _collisionCheckLinkedList[ctrl.Group] = originalNext;
                }
            }
            ctrl.CollisionNext = null;
            ctrl.CollisionPrev = null;
        }

        private void AddToUpdateLinkedList(GameObjectController ctrl)
        {
            var originalHead = _updateHead;
            ctrl.UpdateNext = originalHead;
            ctrl.UpdatePrev = null;
            if (originalHead != null)
            {
                originalHead.UpdatePrev = ctrl;
            }
            _updateHead = ctrl;
        }

        private void RemoveFromUpdateLinkedList(GameObjectController ctrl)
        {
            var originalNext = ctrl.UpdateNext;
            var originalPrev = ctrl.UpdatePrev;
            if (originalNext != null)
            {
                originalNext.UpdatePrev = originalPrev;
                if (originalPrev == null)
                {
                    _updateHead = originalNext;
                }
            }
            if (originalPrev != null)
            {
                originalPrev.UpdateNext = originalNext;
            }
            else
            {
                if (originalNext != null)
                {
                    _updateHead = originalNext;
                }
            }
            ctrl.UpdateNext = null;
            ctrl.UpdatePrev = null;
        }

        public void UpdateXY()
        {
            for (var controller = _updateHead; controller != null; controller = controller.UpdateNext)
            {
                controller.UpdateXY();
            }
        }

        public void UpdateRot()
        {
            for (var controller = _updateHead; controller != null; controller = controller.UpdateNext)
            {
                controller.UpdateRot();
            }
        }

        public void DoFrame()
        {
            for (var controller = _updateHead; controller != null; controller = controller.UpdateNext)
            {
                controller.OnFrame();
            }
        }

        public void CheckBounds()
        {
            for (var controller = _updateHead; controller != null; controller = controller.UpdateNext)
            {
                if (controller.Bound)
                {
                    bool l = controller.X < _bounds.x;
                    bool r = controller.X > _bounds.z;
                    bool b = controller.Y < _bounds.y;
                    bool t = controller.Y > _bounds.w;
                    if ((controller.BoundType & BoundCheckType.VXY) != 0)
                    {
                        l &= controller.VX < 0;
                        r &= controller.VX > 0;
                        b &= controller.VY < 0;
                        t &= controller.VY > 0;
                    }
                    if (l || r || b || t)
                    {
                        controller.Del();
                    }
                }
            }
        }

        public void PerformKill()
        {
            for (var controller = _updateHead; controller != null; )
            {
                var next = controller.UpdateNext;
                if (controller.State == GameObjectState.Dying)
                {
                    Return(controller);
                    if (_updateHead == controller)
                    {
                        _updateHead = next;
                    }
                }
                controller = next;
            }
        }

        public void CollisionCheck()
        {
            State = CallbackState.Collision;
            for (int i = 0; i < COLLISION_GROUP_MAX; i++)
            {
                var mask = _collisionMasks[i];
                var currentMask = 1;
                for (int j = 0; j < COLLISION_GROUP_MAX; j++)
                {
                    if ((mask & currentMask) != 0)
                    {
                        for (var nodeFirst = _collisionCheckLinkedList[i]; nodeFirst != null; nodeFirst = nodeFirst.CollisionNext)
                        {
                            for (var nodeSecond = _collisionCheckLinkedList[j]; nodeSecond != null; nodeSecond = nodeSecond.CollisionNext)
                            {
                                if (_collisionChecker.CheckCollision(nodeFirst, nodeSecond))
                                {
                                    nodeFirst.OnColli(nodeSecond);
                                }
                            }
                        }
                    }
                    currentMask <<= 1;
                }
            }
            State = CallbackState.Idle;
        }
    }
}
