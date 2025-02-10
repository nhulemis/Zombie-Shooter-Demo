
using System;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using Game.Systems.UI;
using UnityEngine;

namespace Game.UI
{
    public class ConfirmQuitDialog : UIBase
    {
        public override async UniTask OnShow(params object[] args)
        {
            await base.OnShow(args);
            // Implement the Show method
        }
        
        public void OnYesButtonClicked()
        {
            // Implement the OnYesButtonClicked method
#if !UNITY_EDITOR
            Application.Quit();
#endif
        }
        
        public void OnNoButtonClicked()
        {
            // Implement the OnNoButtonClicked method
            CloseUI();
        }
    }
}
