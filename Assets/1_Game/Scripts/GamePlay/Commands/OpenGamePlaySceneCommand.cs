using System;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using Game.Systems.UI;
using Game.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _1_Game.Scripts.GamePlay.Commands
{
    public class OpenGamePlaySceneCommand : ICommand
    {
        public async UniTask Execute()
        {
            new OpenLoadingSceneCommand().Execute().Forget();
            var loadingProvider = Locator<LoadingSceneProvider>.Get();
            AsyncOperation operation = SceneManager.LoadSceneAsync("GamePlay", LoadSceneMode.Additive);
            if (operation != null)
            {
                operation.allowSceneActivation = false;
                while (!operation.isDone)
                {
                    loadingProvider.Progress = Mathf.Clamp01(operation.progress / 0.9f);

                    if (operation.progress >= 0.9f) // Scene is ready
                    {
                        new PrepareGameplaySystemCommand().Execute().Forget();
                        Locator<UISystem>.Instance.ExternalCloseUI<LoadingScene>();
                        operation.allowSceneActivation = true;
                    }

                    await UniTask.Yield();
                }
            }
        }
    }
}