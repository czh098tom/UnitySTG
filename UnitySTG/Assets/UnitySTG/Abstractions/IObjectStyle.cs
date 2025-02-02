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
        public void ResetTemplate(GameObject templateInstance, Renderer renderer, Animator animator);
        public GameObject GetTemplate();
        public Renderer UpdateRenderer(GameObject gameObject);
        public Animator UpdateAnimator(GameObject gameObject);
    }
}
