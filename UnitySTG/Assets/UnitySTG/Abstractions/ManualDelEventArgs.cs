﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.Abstractions
{
    public class ManualDelEventArgs : DestroyEventArgs
    {
        public static readonly ManualDelEventArgs Instance = new();
    }
}
