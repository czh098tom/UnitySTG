using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnitySTG.Abstractions.Style
{
    public abstract class SOObjectStyle : ScriptableObject, IObjectStyle
    {
        public abstract GameObject GetTemplate();
        public abstract Animator UpdateAnimator(GameObject gameObject);
        public abstract Renderer UpdateRenderer(GameObject gameObject);
    }
}
