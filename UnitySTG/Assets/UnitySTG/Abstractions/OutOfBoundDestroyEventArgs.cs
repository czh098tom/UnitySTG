using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.Abstractions
{
    public class OutOfBoundDestroyEventArgs : DestroyEventArgs
    {
        public static readonly OutOfBoundDestroyEventArgs Instance = new();
    }
}
