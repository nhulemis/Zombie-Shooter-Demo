using System;
using UnityEngine;

namespace _1_Game.Scripts.Systems.InputSystem
{
    public interface IPlayerInput
    {
        void Initialize();
        Vector3 GetMovement(); 
        bool IsJumping();      
        bool IsAttacking();
    }
}