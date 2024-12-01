using UnityEngine;

using UnitySTG.Abstractions;

namespace UnitySTG.Test
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private LevelController levelController;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            long timer = 0;
            levelController.SetStage(() =>
            {
                if (timer == 60)
                {
                    for (int j = 0; j < 60; j++)
                    {
                        var obj = new LuaSTGObject();
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
