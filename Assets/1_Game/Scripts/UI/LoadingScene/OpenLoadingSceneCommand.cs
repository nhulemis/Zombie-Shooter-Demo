
using System;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using Game.Systems.UI;

namespace Game.UI
{
    [Serializable]
    public class OpenLoadingSceneCommand : ICommand
    {
        public async UniTask Execute()
        {
            Locator<LoadingSceneProvider>.Set(new LoadingSceneProvider());
            await Locator<UISystem>.Instance.Show<LoadingScene>();
            Locator<LoadingSceneProvider>.Release();
            await UniTask.Yield();
        }
    }
}
