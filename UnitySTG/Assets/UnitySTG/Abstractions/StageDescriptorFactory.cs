using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.Abstractions
{
    public class StageDescriptorFactory
    {
        private readonly List<Func<IStageResourceLoadingCallback, IStageResourceLoadingCallback>> _modifyStageResourceLoading = new();
        private readonly List<Func<IStageInitializationCallback, IStageInitializationCallback>> _modifyStageInit = new();
        private readonly List<Func<IStageFrameCallback, IStageFrameCallback>> _modifyStageFrame = new();
        private readonly List<Func<IStageFinishEventBus, IStageFinishEventBus>> _modifyStageFinish = new();

        public StageDescriptorFactory ModifyStageResourceLoading(Func<IStageResourceLoadingCallback, IStageResourceLoadingCallback> func)
        {
            _modifyStageResourceLoading.Add(func);
            return this;
        }

        public StageDescriptorFactory ModifyStageInit(Func<IStageInitializationCallback, IStageInitializationCallback> func)
        {
            _modifyStageInit.Add(func);
            return this;
        }

        public StageDescriptorFactory ModifyStageFrame(Func<IStageFrameCallback, IStageFrameCallback> func)
        {
            _modifyStageFrame.Add(func);
            return this;
        }

        public StageDescriptorFactory ModifyStageFinish(Func<IStageFinishEventBus, IStageFinishEventBus> func)
        {
            _modifyStageFinish.Add(func);
            return this;
        }

        public StageDescriptor Create()
        {
            var init = StageInitializationCallback.Empty;
            foreach (var func in _modifyStageInit)
            {
                init = func(init);
            }

            var frame = StageFrameCallback.Empty;
            foreach (var func in _modifyStageFrame)
            {
                frame = func(frame);
            }

            var load = StageResourceLoadingCallback.Empty;
            foreach (var func in _modifyStageResourceLoading)
            {
                load = func(load);
            }

            IStageFinishEventBus finish = new StageFinishEventBus();
            foreach (var func in _modifyStageFinish)
            {
                finish = func(finish);
            }

            return new StageDescriptor(load, init, frame, finish);
        }
    }
}
