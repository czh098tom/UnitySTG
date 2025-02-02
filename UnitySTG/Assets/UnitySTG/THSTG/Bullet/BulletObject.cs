using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity.Mathematics.FixedPoint;
using UnityEngine;

using UnitySTG.Abstractions;

namespace UnitySTG.THSTG.Bullet
{
    public class BulletObject : LuaSTGObject
    {
        private BulletState _state;
        private int _timer = 0;
        private int? _scheduledTimer;
        private IDisposable _currentDisposable;
        private IBulletStyleSheet _currentStyleSheet;

        public BulletObject(ILevelServiceProvider levelServiceProvider) : base(levelServiceProvider)
        {
            Group = BuiltInGroup.GROUP_ENEMY_BULLET;
            A = 4M;
            B = 4M;

            Layer = BuiltInLayer.LAYER_ENEMY_BULLET;
        }

        public void SetBulletStyleSheet(IBulletStyleSheet bulletStyleSheet)
        {
            Style = bulletStyleSheet.BaseStyle;
            A = (fp)bulletStyleSheet.A;
            B = (fp)bulletStyleSheet.B;

            _currentStyleSheet = bulletStyleSheet;
            var substate = _state switch
            {
                BulletState.Create => bulletStyleSheet.GetSubState(BulletState.Create),
                BulletState.DeadInCreate => bulletStyleSheet.GetSubState(BulletState.DeadInCreate),
                _ => bulletStyleSheet.GetSubState(BulletState.Idle),
            };
            ChangeSubState(substate);
        }

        private void ChangeSubState(IBulletStyleSubState substate)
        {
            _currentDisposable?.Dispose();
            _currentDisposable = substate.OnIntoState(LevelServiceProvider, this);
            _scheduledTimer = substate.GetDuration();
            _timer = 0;
        }

        internal void SetAnimatorVariable(int stateHash)
        {
            Animator.SetTrigger(stateHash);
        }

        protected override void OnFrame()
        {
            base.OnFrame();
            if (_currentStyleSheet == null) return;
            if (_scheduledTimer.HasValue)
            {
                if (_timer >= _scheduledTimer)
                {
                    if (_state == BulletState.Create)
                    {
                        ChangeSubState(_currentStyleSheet.GetSubState(BulletState.Idle));
                        _state = BulletState.Idle;
                    }
                    else if (_state == BulletState.Dead || _state == BulletState.DeadInCreate)
                    {
                        Dispose();
                    }
                    else
                    {
                        _timer++;
                    }
                }
                else
                {
                    _timer++;
                }
            }
            else
            {
                _timer++;
            }
        }

        protected override void OnDel()
        {
            base.OnDel();
            if (_currentStyleSheet == null) return;
            if (_state == BulletState.Create)
            {
                ChangeSubState(_currentStyleSheet.GetSubState(BulletState.DeadInCreate));
                _state = BulletState.DeadInCreate;
            }
            else if (_state == BulletState.Idle)
            {
                ChangeSubState(_currentStyleSheet.GetSubState(BulletState.Dead));
                _state = BulletState.Dead;
            }
        }
    }
}
