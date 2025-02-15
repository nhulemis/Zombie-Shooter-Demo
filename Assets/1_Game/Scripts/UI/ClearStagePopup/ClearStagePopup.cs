
using System;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using Game.Systems.UI;
using UnityEngine;

namespace Game.UI
{
    public class ClearStagePopup : UIBase
    {
        public override async UniTask OnShow(params object[] args)
        {
            await base.OnShow(args);
            // Implement the Show method
        }
        
        public void GoHome()
        {
            // Implement the GoHome method
            
            Locator<UISystem>.Get().ExternalCloseUI<GamePlayScreen>();
        }
    }
}
