using UnityEngine;

using UnitySTG.Abstractions;
using UnitySTG.Abstractions.Style;

namespace UnitySTG.Test
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private LevelController levelController;
        [SerializeField] private DefaultStyle style;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            long timer = 0;
            levelController.SetStage(() =>
            {
                if (timer > 0 && timer % 5 == 0)
                {
                    for (int j = 0; j < 60; j++)
                    {
                        var obj = new LuaSTGObject(levelController)
                        {
                            Style = style
                        };
                        obj.SetV2(3, j * 6);
                    }
                }
                timer++;
            });
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
