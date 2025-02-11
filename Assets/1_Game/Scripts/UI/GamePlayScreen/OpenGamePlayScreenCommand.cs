
using System;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using Game.Systems.UI;

namespace Game.UI
{
    [Serializable]
    public class OpenGamePlayScreenCommand : ICommand
    {
        public async UniTask Execute()
        {
            Locator<GamePlayScreenProvider>.Set(new GamePlayScreenProvider());
            await Locator<UISystem>.Instance.Show<GamePlayScreen>();
            Locator<GamePlayScreenProvider>.Release();
            await UniTask.Yield();
        }
    }
}
