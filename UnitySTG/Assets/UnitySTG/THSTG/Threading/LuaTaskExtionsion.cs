using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.THSTG.Threading
{
    public static class LuaTaskExtionsion
    {
        public static async void Forget(this LuaTask task)
        {
            await task;
        }
    }
}
