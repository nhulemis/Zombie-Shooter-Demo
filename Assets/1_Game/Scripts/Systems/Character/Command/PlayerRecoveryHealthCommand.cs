using System;
using _1_Game.Scripts.GamePlay;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;

namespace _1_Game.Systems.Character.Command
{
    [Serializable]
    public class PlayerRecoveryHealthCommand : ICommand
    {
        public float RecoveryAmount = 100;
        public async UniTask Execute()
        {
            Locator<MapProvider>.Get().PlayerActor.RecoveryHealth(RecoveryAmount);
            await UniTask.CompletedTask;
        }
    }
}