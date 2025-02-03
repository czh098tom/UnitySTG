using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnitySTG.Abstractions;
using UnitySTG.THSTG.Bullet;
using UnitySTG.THSTG.Stage;
using UnitySTG.THSTG.Threading;

namespace UnitySTG.Test
{
    public class TestStage : StageSegment
    {
        private readonly IBulletStyleSheet style;

        public TestStage(IBulletStyleSheet style)
        {
            this.style = style;
        }

        public override void OnInit(ILevelServiceProvider levelServiceProvider)
        {
            base.OnInit(levelServiceProvider);
            DoStage(levelServiceProvider).Forget();
        }

        private async LuaTask DoStage(ILevelServiceProvider levelServiceProvider)
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
    }
}
