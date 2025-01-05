using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

using UnitySTG.Abstractions;
using UnitySTG.Abstractions.Style;
using UnitySTG.Abstractions.ObjectPoolExtension;
using UnitySTG.THSTG;
using UnitySTG.THSTG.Bullet;
using UnitySTG.THSTG.Input;
using UnitySTG.THSTG.Player;
using UnitySTG.THSTG.Stage.Extension;
using UnitySTG.THSTG.UserSettings;

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
            void Script(ILevelServiceProvider _)
            {
                if (timer > 0 && timer % 5 == 0)
                {
                    for (int j = 0; j < 60; j++)
                    {
                        var obj = new BulletBase(levelController)
                        {
                            Style = style
                        };
                        obj.SetV2(3, j * 6);
                    }
                }
                timer++;
            }

            var lifeCycle = new LuaSTGLifeCycleService(_ => UniTask.CompletedTask);
            var factory = new StageDescriptorFactory()
                .ModifyStageInit(init => init.Append(provider =>
                {
                    provider.GetService<UserSettingsProvider>().InputModule = new TestKeyboardInput();
                }))
                .ModifyStageFrame(c => c.Append(Script))
                .AddInputHandling((provider, service) =>
                {
                    service.OpenAxis(TestKeyboardInput.AXIS_X);
                    service.OpenAxis(TestKeyboardInput.AXIS_Y);
                    service.OpenAxis(TestKeyboardInput.AXIS_SLOW);
                    service.OpenAxis(TestKeyboardInput.AXIS_SHOOT);
                    service.OpenAxis(TestKeyboardInput.AXIS_BOMB);
                })
                .AppendStageInit<PlayerHostingService>()
                .ModifyStageInit(init => init.Append(provider =>
                {
                    provider.Pool.SetCollisionCheck(BuiltInGroup.GROUP_PLAYER, BuiltInGroup.GROUP_ENEMY_BULLET, true);
                }))
                .ModifyStageFrame(frame => frame.AttachDefault());

            lifeCycle.LoadAndStartGame(factory.Create(), null, default).SuppressCancellationThrow().Forget();
        }
    }
}
