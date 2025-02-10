
using System;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using Game.Systems.UI;
using UnityEngine;

namespace Game.UI
{
    public class MainMenuScreen : UIBase
    {
        [SerializeReference] private ICommand _beforePlayGameCommand;
        public override async UniTask OnShow(params object[] args)
        {
            await base.OnShow(args);
            // Implement the Show method
        }
        
        public void OnQuitButtonClicked()
        {
            // Implement the OnQuitButtonClicked method
            new OpenConfirmQuitDialogCommand().Execute().Forget();
        }
        
        public void OnArcadeButtonClicked()
        {
            // Implement the OnArcadeButtonClicked method
            _beforePlayGameCommand.Execute().Forget();
            CloseUI();
        }
    }
}
