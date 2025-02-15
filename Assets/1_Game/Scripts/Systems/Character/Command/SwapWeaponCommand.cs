using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _1_Game.Systems.Character.Command
{
    public class SwitchWeaponCommand : ICommand
    {
        public async UniTask Execute()
        {
            PlayerActor player = Object.FindAnyObjectByType<PlayerActor>();
            if (player == null) return;
            player.SwapWeapon();
        }
    }
}