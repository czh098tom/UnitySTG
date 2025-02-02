using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace UnitySTG.Abstractions
{
    public class LuaSTGLifeCycleService
    {
        private readonly Func<CancellationToken, UniTask> _createStageFactory;

        public event EventHandler<StageDescriptor> OnStartLoading;
        public event EventHandler<StageDescriptor> OnLoadingSuccess;
        public event EventHandler<ILevelServiceProvider> OnFinish;
        public event EventHandler<Exception> OnLoadingFailed;

        private StageDescriptor _stageDescriptor;

        public LuaSTGLifeCycleService(Func<CancellationToken, UniTask> createStageFactory)
        {
            _createStageFactory = createStageFactory;
        }

        public async UniTask LoadAndStartGame(StageDescriptor stageDescriptor, IProgress<float> progress, CancellationToken cancellationToken)
        {
            _stageDescriptor = stageDescriptor;
            OnStartLoading?.Invoke(this, stageDescriptor);
            try
            {
                var resourceDictionary = await stageDescriptor.StageResourceLoadingCallback.LoadResources(progress, cancellationToken);
                await _createStageFactory(cancellationToken);
                OnLoadingSuccess?.Invoke(this, stageDescriptor);
                var controller = UnityEngine.Object.FindFirstObjectByType<LevelController>();
                stageDescriptor.StageFinishCallback.OnFinish += (o, e) => OnFinish?.Invoke(o, controller);
                stageDescriptor.StageInitializationCallback.OnInit(controller);
                controller.SetStage(stageDescriptor.StageFrameCallback);
            }
            catch (Exception e)
            {
                OnLoadingFailed?.Invoke(this, e);
            }
        }
    }
}
