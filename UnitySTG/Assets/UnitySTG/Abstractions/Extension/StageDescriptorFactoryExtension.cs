using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.Abstractions.Extension
{
    public static class StageDescriptorFactoryExtension
    {
        public static StageDescriptorFactory AppendStageInit<T>(this StageDescriptorFactory factory,
            Action<ILevelServiceProvider, T> configure = null)
            where T : class, IStageInitializationCallback
        {
            return factory.ModifyStageInit(si =>
            {
                return si.Append(provider =>
                {
                    var service = provider.GetService<T>();
                    service.OnInit(provider);
                    configure?.Invoke(provider, service);
                });
            });
        }

        public static StageDescriptorFactory PrependStageFrameCached<T>(this StageDescriptorFactory factory)
            where T : class, IStageFrameCallback
        {
            return factory.ModifyStageFrame(si =>
            {
                T service = null;
                return si.Prepend(provider =>
                {
                    service ??= provider.GetService<T>();
                    service.OnFrame(provider);
                });
            });
        }

        public static StageDescriptorFactory AppendStageFrameCached<T>(this StageDescriptorFactory factory)
            where T : class, IStageFrameCallback
        {
            return factory.ModifyStageFrame(si =>
            {
                T service = null;
                return si.Append(provider =>
                {
                    service ??= provider.GetService<T>();
                    service.OnFrame(provider);
                });
            });
        }
    }
}
