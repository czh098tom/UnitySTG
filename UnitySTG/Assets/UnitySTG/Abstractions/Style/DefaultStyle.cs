using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnitySTG.Abstractions.Style
{
    public class DefaultStyle : SOObjectStyle
    {
        [SerializeField] private GameObject _template;

        public override GameObject GetTemplate()
        {
            return _template;
        }

        public override void ResetTemplate(GameObject templateInstance, Renderer renderer, Animator animator)
        {
            templateInstance.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            templateInstance.transform.localScale = Vector3.one;
        }

        public override Animator UpdateAnimator(GameObject gameObject)
        {
            var anim = gameObject.GetComponent<Animator>();
            return anim;
        }

        public override Renderer UpdateRenderer(GameObject gameObject)
        {
            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            return spriteRenderer;
        }
    }
}
