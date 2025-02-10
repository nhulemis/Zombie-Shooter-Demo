using System;
using UnityEngine;

namespace _1_Game.Scripts.Systems.InputSystem
{
    [Serializable]
    public class MobileInput : IPlayerInput
    {
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