
using System;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using Game.Systems.UI;

namespace Game.UI
{
    [Serializable]
    public class OpenMainMenuScreenCommand : ICommand
    {
        public async UniTask Execute()
        {
            Locator<MainMenuScreenProvider>.Set(new MainMenuScreenProvider());
            await Locator<UISystem>.Instance.Show<MainMenuScreen>();
            Locator<MainMenuScreenProvider>.Release();
            await UniTask.Yield();
        }
    }
}
