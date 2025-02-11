
using System;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using Game.Systems.UI;

namespace Game.UI
{
    [Serializable]
    public class OpenMobileInputViewCommand : ICommand
    {
        public async UniTask Execute()
        {
            Locator<MobileInputViewProvider>.Set(new MobileInputViewProvider());
            await Locator<UISystem>.Instance.Show<MobileInputView>();
            Locator<MobileInputViewProvider>.Release();
            await UniTask.Yield();
        }
    }
}
