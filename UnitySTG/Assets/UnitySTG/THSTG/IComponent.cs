using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitySTG.Abstractions;

namespace UnitySTG.THSTG
{
    public interface IComponent
    {
        void OnColli(LuaSTGObject other);
        void OnFrame();
        void OnDel();
    }
}
