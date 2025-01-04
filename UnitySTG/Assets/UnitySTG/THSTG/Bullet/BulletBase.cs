using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnitySTG.Abstractions;

namespace UnitySTG.THSTG.Bullet
{
    public class BulletBase : LuaSTGObject
    {
        public BulletBase(ILevelServiceProvider levelServiceProvider) : base(levelServiceProvider)
        {
            Group = BuiltInGroup.GROUP_ENEMY_BULLET;
            A = 4M;
            B = 4M;

            Layer = BuiltInLayer.LAYER_ENEMY_BULLET;
        }
    }
}
