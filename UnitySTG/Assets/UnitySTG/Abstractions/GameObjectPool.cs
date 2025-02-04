﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

using Unity.Mathematics.FixedPoint;

namespace UnitySTG.Abstractions
{
    public class GameObjectPool : MonoBehaviour
    {
        internal static readonly byte COLLISION_GROUP_MAX = 32;
        internal static readonly int COMPRESS_THRESHOLD = int.MaxValue / 2;

        internal enum CallbackState
        {
            Idle,
            Collision,
        }

        [SerializeField] private LevelController _levelController;
        [SerializeField] private GameObjectController _prefab;
        [SerializeField] private int _maxSize = 65536;

        internal CallbackState State { get; private set; }

        private int _currObjID = 0;
        private int _currObjCount = 0;

        private fp4 _bounds = new(-224, -256, 224, 256);

        private readonly CollisionChecker _collisionChecker = new();

        private ObjectPool<GameObjectController> _pool;
        private readonly GameObjectController[] _collisionCheckLinkedList = new GameObjectController[COLLISION_GROUP_MAX];
        private GameObjectController _updateHead;

        private uint[] _collisionMasks = new uint[COLLISION_GROUP_MAX];

        private void Awake()
        {
            _pool = new(() =>
            {
                return Instantiate(_prefab, transform);
            }, actionOnGet: ctrl =>
            {
                _currObjID++;
                _currObjCount++;
                ctrl.gameObject.SetActive(true);
                ctrl.OnCreated(_levelController, _currObjID);
                AddToCollisionCheckLinkedList(ctrl);
                AddToUpdateLinkedList(ctrl);
            }, actionOnRelease: ctrl =>
            {
                _currObjCount--;
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
            var originalHead = _collisionCheckLinkedList[ctrl.Group];
            ctrl.CollisionNext = originalHead;
            ctrl.CollisionPrev = null;
            if (originalHead != null)
            {
                originalHead.CollisionPrev = ctrl;
            }
            ctrl.InCollisionList = true;
            _collisionCheckLinkedList[ctrl.Group] = ctrl;
        }

        internal void RemoveFromCollisionCheckLinkedList(GameObjectController ctrl)
        {
            var originalNext = ctrl.CollisionNext;
            var originalPrev = ctrl.CollisionPrev;
            if (ctrl.InCollisionList)
            {
                if (originalNext != null)
                {
                    if (originalPrev != null)
                    {
                        originalNext.CollisionPrev = originalPrev;
                        originalPrev.CollisionNext = originalNext;
                    }
                    else
                    {
                        originalNext.CollisionPrev = originalPrev;
                        _collisionCheckLinkedList[ctrl.Group] = originalNext;
                    }
                }
                else
                {
                    if (originalPrev != null)
                    {
                        originalPrev.CollisionNext = originalNext;
                    }
                    else
                    {
                        _collisionCheckLinkedList[ctrl.Group] = originalNext;
                    }
                }
                ctrl.InCollisionList = false;
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
            ctrl.InUpdateList = true;
            _updateHead = ctrl;
        }

        private void RemoveFromUpdateLinkedList(GameObjectController ctrl)
        {
            var originalNext = ctrl.UpdateNext;
            var originalPrev = ctrl.UpdatePrev;
            if (ctrl.InUpdateList)
            {
                if (originalNext != null)
                {
                    if (originalPrev != null)
                    {
                        originalNext.UpdatePrev = originalPrev;
                        originalPrev.UpdateNext = originalNext;
                    }
                    else
                    {
                        originalNext.UpdatePrev = originalPrev;
                        _updateHead = originalNext;
                    }
                }
                else
                {
                    if (originalPrev != null)
                    {
                        originalPrev.UpdateNext = originalNext;
                    }
                    else
                    {
                        _updateHead = originalNext;
                    }
                }
                ctrl.InUpdateList = false;
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
                        controller.Destroy(OutOfBoundDestroyEventArgs.Instance);
                    }
                }
            }
        }

        public void PerformKill()
        {
            for (var controller = _updateHead; controller != null;)
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

        public void SetCollisionCheck(byte first, byte second, bool isOn)
        {
            if (first < 0 && first >= COLLISION_GROUP_MAX) return;
            if (second < 0 && second >= COLLISION_GROUP_MAX) return;
            var v = _collisionMasks[first];
            _collisionMasks[first] = v & ~((uint)0b1 << second);
            if (isOn)
            {
                _collisionMasks[first] |= (uint)0b1 << second;
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

        public void TryCompressObjectID()
        {
            if (_currObjID > COMPRESS_THRESHOLD && _currObjCount < byte.MaxValue)
            {
                GameObjectController last = _updateHead;
                while (last != null && last.UpdateNext != null) last = last.UpdateNext;
                GameObjectController curr = last;
                int i = 0;
                while (curr != null)
                {
                    curr.ObjectID = i;
                    curr = curr.UpdatePrev;
                }
                _currObjID = i;
            }
        }

#if UNITY_EDITOR
        [field: SerializeField] public ColliderGizmoDescriptor ColliderGizmoDescriptor { get; set; }
#endif
    }
}
