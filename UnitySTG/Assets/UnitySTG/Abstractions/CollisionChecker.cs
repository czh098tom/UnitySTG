using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.Abstractions
{
    internal class CollisionChecker
    {
        public bool CheckCollision(GameObjectController first, GameObjectController second)
        {
            return (first.ColliderShape, second.ColliderShape) switch
            {
                (ColliderShape.Circular, ColliderShape.Circular) => CheckAsCircle(first, second),
                (_, _) => CheckAsCircle(first, second),
            };
        }

        private bool CheckAsCircle(GameObjectController first, GameObjectController second)
        {
            var sum = first.A + second.A;
            var x = second.X - first.X;
            var y = second.Y - first.Y;
            return sum * sum >= x * x + y * y;
        }
    }
}
