using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using UnitySTG.Abstractions;
using UnitySTG.THSTG.Input;

namespace UnitySTG.THSTG.Player
{
    public class PlayerHostingService : MonoBehaviour, IStageInitializationCallback
    {
        public void OnInit(ILevelServiceProvider levelServiceProvider)
        {
            new PlayerBase(levelServiceProvider, this, levelServiceProvider.GetService<InputHandlingService>());
        }
    }
}
