using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace UnitySTG.Abstractions
{
    public class LuaSTGLifeCycleService : IStageFinishCallback
    {
        private readonly Func<CancellationToken, UniTask> _createStageFactory;

        public event EventHandler<StageDescriptor> OnStartLoading;
        public event EventHandler<StageDescriptor> OnLoadingSuccess;
        public event EventHandler<Exception> OnLoadFailed;

        private StageDescriptor _stageDescriptor;

        public LuaSTGLifeCycleService(Func<CancellationToken, UniTask> createStageFactory)
        {
            _createStageFactory = createStageFactory;
        }

        public async UniTask LoadAndStartGame(StageDescriptor stageDescriptor, CancellationToken cancellationToken)
        {
            _stageDescriptor = stageDescriptor;
            OnStartLoading?.Invoke(this, stageDescriptor);
            try
            {
                var resourceDictionary = await stageDescriptor.StageResourceLoadingCallback.LoadResources(cancellationToken);
                await _createStageFactory(cancellationToken);
                OnLoadingSuccess?.Invoke(this, stageDescriptor);
                var controller = UnityEngine.Object.FindFirstObjectByType<LevelController>();
                controller.LifeCycle = stageDescriptor.StageFinishCallback;
                stageDescriptor.StageInitializationCallback.OnInit(controller);
                controller.SetStage(stageDescriptor.StageFrameCallback.OnFrame);
            }
            catch (Exception e)
            {
                OnLoadFailed?.Invoke(this, e);
            }
        }

        public void OnFinish(ILevelServiceProvider levelServiceProvider)
        {
            _stageDescriptor = null;
        }
    }
}
