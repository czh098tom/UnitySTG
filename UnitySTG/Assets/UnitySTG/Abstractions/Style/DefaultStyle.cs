using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnitySTG.Abstractions.Style
{
    public class DefaultStyle : ScriptableObject, IObjectStyle
    {
        [SerializeField] private GameObject _template;

        public GameObject GetTemplate()
        {
            return _template;
        }
        public virtual Animator GetAnimator(GameObject gameObject)
        {
            var anim = gameObject.GetComponent<Animator>();
            return anim;
        }

        public virtual Renderer GetRenderer(GameObject gameObject)
        {
            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            return spriteRenderer;
        }
    }
}
