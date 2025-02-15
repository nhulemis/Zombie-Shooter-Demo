
using System;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using Game.Systems.UI;

namespace Game.UI
{
    [Serializable]
    public class OpenClearStagePopupCommand : ICommand
    {
        public async UniTask Execute()
        {
            Locator<ClearStagePopupProvider>.Set(new ClearStagePopupProvider());
            await Locator<UISystem>.Instance.Show<ClearStagePopup>();
            Locator<ClearStagePopupProvider>.Release();
            await UniTask.Yield();
        }
    }
}
