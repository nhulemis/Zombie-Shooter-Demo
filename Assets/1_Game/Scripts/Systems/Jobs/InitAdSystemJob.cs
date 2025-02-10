using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _1_Game.Scripts.Systems
{
    public class InitAdSystemJob : ICommand
    {
        public async UniTask Execute()
        {
            var go = new GameObject("AdSystem").AddComponent<AdSystem>();
            Locator<AdSystem>.Set(go);
            await UniTask.CompletedTask;
        }
    }
}