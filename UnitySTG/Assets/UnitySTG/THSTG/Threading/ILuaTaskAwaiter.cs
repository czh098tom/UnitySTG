using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.THSTG.Threading
{
    public interface ILuaTaskAwaiter : INotifyCompletion
    {
        public bool IsCompleted { get; }
        public bool IsFaulted { get; }

        public void GetResult();
        public void TrySetResult();
        public void TrySetException(Exception e);
    }
}
