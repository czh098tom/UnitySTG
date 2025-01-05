using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using UnitySTG.THSTG.Input;

namespace UnitySTG.THSTG.Stage
{
    public class StageConfigurationProvider : MonoBehaviour
    {
        public IReplayReader ReplayReader { get; private set; }
        public IReplayWriter ReplayWriter { get; private set; }
    }
}
