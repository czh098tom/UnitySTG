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
    public class BulletController : ComponentBase
    {
        private BulletState _state;
        private int _timer = 0;
        private int? _scheduledTimer;
        private IDisposable _currentDisposable;
        private IBulletStyleSheet _currentStyleSheet;

        public void SetBulletStyleSheet(IBulletStyleSheet bulletStyleSheet)
        {
            LuaSTGObject.Style = bulletStyleSheet.BaseStyle;
            LuaSTGObject.A = (fp)bulletStyleSheet.A;
            LuaSTGObject.B = (fp)bulletStyleSheet.B;

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
            _currentDisposable = substate.OnIntoState(LuaSTGObject.LevelServiceProvider, this);
            _scheduledTimer = substate.GetDuration();
            _timer = 0;
        }

        internal void SetAnimatorVariable(int stateHash)
        {
            LuaSTGObject.Animator.SetTrigger(stateHash);
        }

        public override void OnAttach()
        {
            base.OnAttach();
            LuaSTGObject.Group = BuiltInGroup.GROUP_ENEMY_BULLET;
            LuaSTGObject.A = 4M;
            LuaSTGObject.B = 4M;

            LuaSTGObject.Layer = BuiltInLayer.LAYER_ENEMY_BULLET;
        }

        public override void OnFrame()
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
                        LuaSTGObject.Dispose();
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

        public override void OnDel()
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
