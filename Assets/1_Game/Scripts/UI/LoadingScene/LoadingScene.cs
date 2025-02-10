
using System;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using Game.Systems.UI;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.UI
{
    public class LoadingScene : UIBase
    {
        [SerializeField] TMP_Text _progressText;
        public LoadingSceneProvider LoadingSceneProvider => Locator<LoadingSceneProvider>.Get();
        public override async UniTask OnShow(params object[] args)
        {
            await base.OnShow(args);
            // Implement the Show method
            LoadingSceneProvider.RxProgress.Subscribe(Value =>
            {
                _progressText.text = $"Loading... {Value * 100}%";
            }).AddTo(this);
        }
    }
}
