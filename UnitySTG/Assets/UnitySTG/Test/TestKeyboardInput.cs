using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics.FixedPoint;

using UnityEngine;

using UnitySTG.THSTG.Input;

namespace UnitySTG.Test
{
    public class TestKeyboardInput : IInputModule
    {
        public static readonly int AXIS_X = 0;
        public static readonly int AXIS_Y = 1;
        public static readonly int AXIS_SLOW = 2;
        public static readonly int AXIS_SHOOT = 3;
        public static readonly int AXIS_BOMB = 4;

        private readonly InputAxis[] axes = new InputAxis[5];

        public Span<InputAxis> ConvertAxis()
        {
            int count = 0;
            int x = 0, y = 0;
            if (Input.GetKey(KeyCode.LeftArrow)) x -= 1;
            if (Input.GetKey(KeyCode.RightArrow)) x += 1;
            if (Input.GetKey(KeyCode.DownArrow)) y -= 1;
            if (Input.GetKey(KeyCode.UpArrow)) y += 1;
            fp scale = 1M;
            if (x != 0 && y != 0)
            {
                scale = fpmath.SQRT2 / 2M;
            }
            if (x != 0)
            {
                axes[count] = new InputAxis(AXIS_X, x * scale);
                count++;
            }
            if (y!= 0)
            {
                axes[count] = new InputAxis(AXIS_Y, y * scale);
                count++;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                axes[count] = new InputAxis(AXIS_SLOW, 1);
                count++;
            }
            if (Input.GetKey(KeyCode.Z))
            {
                axes[count] = new InputAxis(AXIS_SHOOT, 1);
                count++;
            }
            if (Input.GetKey(KeyCode.X))
            {
                axes[count] = new InputAxis(AXIS_BOMB, 1);
                count++;
            }

            return new Span<InputAxis>(axes, 0, count);
        }
    }
}
