using _1_Game.Scripts.Systems;
using _1_Game.Scripts.Systems.AddressableSystem;
using _1_Game.Scripts.Systems.AI.PathFinding;
using _1_Game.Scripts.Systems.Observe;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using Game.Systems.UI;
using Game.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _1_Game.Scripts.GamePlay.Commands
{
    public class PrepareOutGameplayCommand : ICommand
    {
        public async UniTask Execute()
        {
            Locator<InventorySystem>.Release();
            Locator<NavMeshProvider>.Release();
            Locator<DoorObserver>.Release();
            Locator<MapProvider>.Release();
            new OpenLoadingSceneCommand().Execute().Forget();
            Locator<UISystem>.Instance.ExternalCloseUI<ClearStagePopup>();
            Locator<UISystem>.Instance.ExternalCloseUI<GamePlayScreen>();
            Locator<UISystem>.Instance.ExternalCloseUI<MissionFailPopup>();
            Locator<UISystem>.Instance.ExternalCloseUI<MobileInputView>();
            Locator<UISystem>.Instance.ExternalCloseUI<DesktopInputView>();
            
            var operation = SceneManager.UnloadSceneAsync("GamePlay", UnloadSceneOptions.None);
            var loadingProvider = Locator<LoadingSceneProvider>.Get();

            if (operation != null)
            {
                operation.allowSceneActivation = false;
                while (!operation.isDone)
                {
                    loadingProvider.Progress = Mathf.Clamp01(operation.progress / 0.9f);
                    await UniTask.Yield();
                }
            }
            new OpenMainMenuScreenCommand().Execute().Forget();
            Locator<UISystem>.Instance.ExternalCloseUI<LoadingScene>();

            await UniTask.CompletedTask;
        }
    }
}