using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics.FixedPoint;

using UnitySTG.Abstractions;
using UnitySTG.THSTG.Input;

namespace UnitySTG.THSTG.Player
{
    public class PlayerBase : LuaSTGObject
    {
        private readonly PlayerHostingService playerHostingService;
        private readonly InputHandlingService inputHandlingService;

        public PlayerBase(ILevelServiceProvider levelServiceProvider,
            PlayerHostingService playerHostingService,
            InputHandlingService inputHandlingService) : base(levelServiceProvider)
        {
            this.playerHostingService = playerHostingService;
            this.inputHandlingService = inputHandlingService;
        }

        protected override void OnFrame()
        {
            base.OnFrame();
            var x = inputHandlingService.GetAxis(0);
            var y = inputHandlingService.GetAxis(1);
            var slow = inputHandlingService.GetAxis(2) > 0;
            fp hspeed = 4;
            fp lspeed = 2;

            var speed = slow ? lspeed : hspeed;

            X += speed * x;
            Y += speed * y;
        }
    }
}
