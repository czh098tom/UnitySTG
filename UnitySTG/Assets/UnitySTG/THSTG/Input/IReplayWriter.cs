using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.THSTG.Input
{
    public interface IReplayWriter
    {
        public void Write(Span<InputAxis> axis);
    }
}
