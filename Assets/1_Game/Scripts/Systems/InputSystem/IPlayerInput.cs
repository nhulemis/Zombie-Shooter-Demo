using System;
using UnityEngine;

namespace _1_Game.Scripts.Systems.InputSystem
{
    public interface IPlayerInput
    {
        Vector3 GetMovement(); 
        bool IsJumping();      
        bool IsAttacking();
    }
}