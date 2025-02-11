using System;
using Cysharp.Threading.Tasks;
using Game.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _1_Game.Scripts.Systems.InputSystem
{
    [Serializable]
    public class KeyBoardInput : IPlayerInput
    {
        public void Initialize()
        {
            new OpenDesktopInputViewCommand().Execute().Forget();
        }

        public Vector3 GetMovement()
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveY = Input.GetAxis("Vertical");
            return new Vector3(moveX, 0 , moveY);
        }

        public bool IsJumping()
        {
            return Keyboard.current.spaceKey.isPressed;
        }

        public bool IsAttacking()
        {
            throw new System.NotImplementedException();
        }
    }
}