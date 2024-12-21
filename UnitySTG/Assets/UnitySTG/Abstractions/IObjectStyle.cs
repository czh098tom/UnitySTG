using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnitySTG.Abstractions
{
    public interface IObjectStyle
    {
        public GameObject GetTemplate();
        public Renderer GetRenderer(GameObject gameObject);
        public Animator GetAnimator(GameObject gameObject);
    }
}
