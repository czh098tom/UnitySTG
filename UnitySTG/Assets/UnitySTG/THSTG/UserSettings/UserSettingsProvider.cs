using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using UnitySTG.THSTG.Input;

namespace UnitySTG.THSTG.UserSettings
{
    public class UserSettingsProvider : MonoBehaviour
    {
        public IInputModule InputModule { get; set; }
    }
}
