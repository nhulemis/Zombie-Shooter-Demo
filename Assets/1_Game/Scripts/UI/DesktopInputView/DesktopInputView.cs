
using System;
using Cysharp.Threading.Tasks;
using Game.Systems.UI;
using UnityEngine;

namespace Game.UI
{
    public class DesktopInputView : UIBase
    {
        public override async UniTask OnShow(params object[] args)
        {
            await base.OnShow(args);
            // Implement the Show method
        }
    }
}
