using System;
using UnityEngine;

namespace _1_Game.Systems.Character
{
    public class CharacterAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float acceleration = 0.1f;
        [SerializeField] private float deceleration = 0.5f;
        
        private const string _layerAiming = "Aiming";
        private int _velocityHash = Animator.StringToHash("Velocity");
        private float _velocity;
        

        private void OnValidate()
        {
            if (_animator == null)
            {
                _animator = GetComponentInChildren<Animator>();
            }
        }

        public void EquipWeapon()
        {
            int layerAimingIndex = _animator.GetLayerIndex(_layerAiming);
            _animator.SetLayerWeight(layerAimingIndex, 1);
        }
        
        public void Execute_MovementAnimation(Vector3 movement)
        {
            bool isMoving = movement.x != 0 || movement.z != 0;

            if (isMoving && _velocity < 1)
            {
                _velocity += acceleration * Time.fixedDeltaTime;
            }
            else if (!isMoving && _velocity > 0)
            {
                _velocity -= deceleration * Time.fixedDeltaTime;
            }
            
            if(!isMoving && _velocity < 0)
            {
                _velocity = 0;
            }
            _animator.SetFloat(_velocityHash, _velocity);
        }
    }
}