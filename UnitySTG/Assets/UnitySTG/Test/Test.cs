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
using UnitySTG.THSTG.Threading;
using UnitySTG.THSTG.Stage;

namespace UnitySTG.Test
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private SOBulletStyleSheet style;

        void Start()
        {
            var exec = false;
            void Script(ILevelServiceProvider levelServiceProvider)
            {
                if (!exec)
                {
                    Run(levelServiceProvider).Forget();
                    exec = true;
                }
            }

            async LuaTask Run(ILevelServiceProvider levelServiceProvider)
            {
                int i = 0;
                while (true)
                {
                    await LuaTask.Delay(5);
                    for (int j = 0; j < 60; j++)
                    {
                        var obj = new BulletObject(levelServiceProvider);
                        obj.SetBulletStyleSheet(style);
                        obj.SetV2(3, j * 6 + i * 6 * 0.618M);
                    }
                    i++;
                }
            }

            var lifeCycle = new LuaSTGLifeCycleService(_ => UniTask.CompletedTask);
            var factory = new StageDescriptorFactory()
                .ModifyStageInit(init => init.Append(provider =>
                {
                    provider.GetService<UserSettingsProvider>().InputModule = new TestKeyboardInput();
                }))
                .ModifyStageFrame(c => c.Append(Script))
                .PrependStageFrameCached<LuaTaskScheduler>()
                .AddInputHandling((provider, service) =>
                {
                    service.OpenAxis(TestKeyboardInput.AXIS_X);
                    service.OpenAxis(TestKeyboardInput.AXIS_Y);
                    service.OpenAxis(TestKeyboardInput.AXIS_SLOW);
                    service.OpenAxis(TestKeyboardInput.AXIS_SHOOT);
                    service.OpenAxis(TestKeyboardInput.AXIS_BOMB);
                })
                .AppendStageInit<PlayerHostingService>()
                .AppendStageInit<LuaTaskScheduler>()
                .AppendStageInit<PauseController>()
                .ModifyStageInit(init => init.Append(provider =>
                {
                    provider.Pool.SetCollisionCheck(BuiltInGroup.GROUP_PLAYER, BuiltInGroup.GROUP_ENEMY_BULLET, true);
                }))
                .ModifyStageFrame(frame => frame.AttachDefault());

            lifeCycle.OnLoadingFailed += (o, e) => Debug.LogException(e);
            lifeCycle.LoadAndStartGame(factory.Create(), null, default).SuppressCancellationThrow().Forget();
        }
    }
}
