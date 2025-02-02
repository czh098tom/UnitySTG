using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using UnitySTG.Abstractions.Style;

namespace UnitySTG.THSTG.Style
{
    public class SOImageObjectStyle : SOObjectStyle
    {
        [SerializeField] private GameObject _template;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private Color _color = Color.white;
        [SerializeField] private RuntimeAnimatorController _animatorController;

        public override GameObject GetTemplate()
        {
            return _template;
        }

        public override void ResetTemplate(GameObject templateInstance, Renderer renderer, Animator animator)
        {
            animator.runtimeAnimatorController = null;
            if (renderer is SpriteRenderer spriteRenderer)
            {
                spriteRenderer.color = Color.white;
            }
            templateInstance.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            templateInstance.transform.localScale = Vector3.one;
        }

        public override Animator UpdateAnimator(GameObject gameObject)
        {
            var anim = gameObject.GetComponent<Animator>();
            anim.runtimeAnimatorController = _animatorController;
            return anim;
        }

        public override Renderer UpdateRenderer(GameObject gameObject)
        {
            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = _sprite;
            spriteRenderer.color = _color;
            return spriteRenderer;
        }
    }
}
