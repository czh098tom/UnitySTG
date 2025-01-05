using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics.FixedPoint;
using UnityEngine;

using UnitySTG.Abstractions;
using UnitySTG.Abstractions.Style;
using UnitySTG.THSTG.Input;

namespace UnitySTG.THSTG.Player
{
    public class PlayerHostingService : MonoBehaviour, IStageInitializationCallback
    {
        [SerializeField] private DefaultStyle style;

        private fp2 boundsMin = new(-192 + 8, -224 + 16);
        private fp2 boundsMax = new(192 - 8, 224 - 32);

        public fp2 BoundsMin => boundsMin;
        public fp2 BoundsMax => boundsMax;

        public void OnInit(ILevelServiceProvider levelServiceProvider)
        {
            new PlayerBase(levelServiceProvider, this, levelServiceProvider.GetService<InputHandlingService>())
            {
                Style = style
            };
        }

        public void SetBounds(fp2 min, fp2 max)
        {
            boundsMin = min;
            boundsMax = max;
        }
    }
}
