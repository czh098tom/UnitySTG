using UnityEngine;

using UnitySTG.Abstractions;
using UnitySTG.Abstractions.Style;
using UnitySTG.THSTG.Input;
using UnitySTG.THSTG.Player;

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
            InputHandlingService inputHandlingService = null;
            PlayerHostingService playerHostingService = null;
            levelController.SetStage(levelServiceProvider =>
            {
                if (inputHandlingService == null)
                {
                    inputHandlingService = levelServiceProvider.GetService<InputHandlingService>();
                    inputHandlingService.InputModule = new TestKeyboardInput();
                    inputHandlingService.OpenAxis(TestKeyboardInput.AXIS_X);
                    inputHandlingService.OpenAxis(TestKeyboardInput.AXIS_Y);
                    inputHandlingService.OpenAxis(TestKeyboardInput.AXIS_SLOW);
                    inputHandlingService.OpenAxis(TestKeyboardInput.AXIS_SHOOT);
                    inputHandlingService.OpenAxis(TestKeyboardInput.AXIS_BOMB);
                }
                if (playerHostingService == null)
                {
                    playerHostingService = levelServiceProvider.GetService<PlayerHostingService>();
                    playerHostingService.OnInit(levelServiceProvider);
                }

                inputHandlingService.OnFrame(levelServiceProvider);

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
