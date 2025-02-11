using System;
using Cysharp.Threading.Tasks;
using Game.UI;
using UnityEngine;

namespace _1_Game.Scripts.Systems.InputSystem
{
    [Serializable]
    public class MobileInput : IPlayerInput
    {
        public void Initialize()
        {
            new OpenMobileInputViewCommand().Execute().Forget();
        }

        public Vector3 GetMovement()
        {
            throw new System.NotImplementedException();
        }

        public bool IsJumping()
        {
            throw new System.NotImplementedException();
        }

        public bool IsAttacking()
        {
            throw new System.NotImplementedException();
        }
    }
}