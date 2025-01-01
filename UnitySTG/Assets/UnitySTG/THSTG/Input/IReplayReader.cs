using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.THSTG.Input
{
    public interface IReplayReader
    {
        public Span<InputAxis> OnNext();
    }
}
