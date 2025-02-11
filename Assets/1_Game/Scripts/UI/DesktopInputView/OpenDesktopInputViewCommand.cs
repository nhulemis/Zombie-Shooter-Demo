
using System;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using Game.Systems.UI;

namespace Game.UI
{
    [Serializable]
    public class OpenDesktopInputViewCommand : ICommand
    {
        public async UniTask Execute()
        {
            Locator<DesktopInputViewProvider>.Set(new DesktopInputViewProvider());
            await Locator<UISystem>.Instance.Show<DesktopInputView>();
            Locator<DesktopInputViewProvider>.Release();
            await UniTask.Yield();
        }
    }
}
