using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnitySTG.Abstractions;

namespace UnitySTG.THSTG
{
    public class ComponentBase : IComponent
    {
        public LuaSTGObject LuaSTGObject { get; set; }

        public virtual void OnAttach()
        {
        }

        public virtual void OnColli(LuaSTGObject other)
        {
        }

        public virtual void OnDel()
        {
        }

        public virtual void OnFrame()
        {
        }

        public void Dispose()
        {
            Dispose(true);
        }

        ~ComponentBase()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                OnDel();
            }
        }
    }
}
