using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.Abstractions.Progress
{
    public class OffsetProgress : IProgress<ResourceLoadInfo>
    {
        private readonly IProgress<ResourceLoadInfo> inner;
        private readonly float _start;
        private readonly float _end;

        public OffsetProgress(IProgress<ResourceLoadInfo> inner, float start, float end)
        {
            this.inner = inner;
            _start = start;
            _end = end;
        }

        public void Report(ResourceLoadInfo value)
        {
            inner.Report(value with { Progress = (_end - _start) * value.Progress + _start });
        }
    }
}
