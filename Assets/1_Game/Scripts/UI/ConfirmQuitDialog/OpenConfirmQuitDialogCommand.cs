
using System;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using Game.Systems.UI;

namespace Game.UI
{
    [Serializable]
    public class OpenConfirmQuitDialogCommand : ICommand
    {
        public async UniTask Execute()
        {
            Locator<ConfirmQuitDialogProvider>.Set(new ConfirmQuitDialogProvider());
            await Locator<UISystem>.Instance.Show<ConfirmQuitDialog>();
            Locator<ConfirmQuitDialogProvider>.Release();
            await UniTask.Yield();
        }
    }
}
