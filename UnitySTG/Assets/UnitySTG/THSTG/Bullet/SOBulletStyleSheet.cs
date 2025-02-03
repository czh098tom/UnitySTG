using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics.FixedPoint;
using UnityEngine;

using UnitySTG.Abstractions;
using UnitySTG.Abstractions.Style;

namespace UnitySTG.THSTG.Bullet
{
    [Serializable]
    public class SOBulletStyleSheet : ScriptableObject, IBulletStyleSheet
    {
        [field: SerializeField] public float A { get; private set; } = 4;
        [field: SerializeField] public float B { get; private set; } = 4;

        [SerializeField] private SOObjectStyle baseStyle;
        [SerializeField] private SOBulletStyleSelector create;
        [SerializeField] private SOBulletStyleSelector idle;
        [SerializeField] private SOBulletStyleSelector dead;
        [SerializeField] private SOBulletStyleSelector deadInCreate;

        IObjectStyle IBulletStyleSheet.BaseStyle => baseStyle;

        public IBulletStyleSubState GetSubState(BulletState state)
        {
            return state switch
            {
                BulletState.Create => create,
                BulletState.Idle => idle,
                BulletState.Dead => dead,
                BulletState.DeadInCreate => deadInCreate,
                _ => idle,
            };
        }
    }
}
