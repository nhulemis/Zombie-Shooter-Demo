
using System;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using Game.Systems.UI;

namespace Game.UI
{
    [Serializable]
    public class OpenMissionFailPopupCommand : ICommand
    {
        public async UniTask Execute()
        {
            Locator<MissionFailPopupProvider>.Set(new MissionFailPopupProvider());
            await Locator<UISystem>.Instance.Show<MissionFailPopup>();
            Locator<MissionFailPopupProvider>.Release();
            await UniTask.Yield();
        }
    }
}
