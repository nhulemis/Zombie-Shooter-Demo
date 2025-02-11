using System;
using _1_Game.Scripts.Systems.WeaponSystem;
using UnityEngine;

namespace _1_Game.Systems.Character
{
    public class CharacterAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float acceleration = 0.1f;
        [SerializeField] private float deceleration = 0.5f;
        
        private int _velocityHash = Animator.StringToHash("Velocity");
        private float _velocity;

        private void OnValidate()
        {
            if (_animator == null)
            {
                _animator = GetComponentInChildren<Animator>();
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            ResetWeightLayer();
            int layerAimingIndex = _animator.GetLayerIndex(weapon.PoseLayerName);
            _animator.SetLayerWeight(layerAimingIndex, 1);
        }
        
        private void ResetWeightLayer()
        {
            if(_animator.layerCount <= 1) return;
            for (int i = 1; i < _animator.layerCount; i++)
            {
                _animator.SetLayerWeight(i, 0);
            }
        }
        
        public void Execute_MovementAnimation(Vector3 movement, bool isAiming = false)
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

            float maxVelocity = isAiming ? 0.5f : 1;
            _velocity = Mathf.Clamp(_velocity, 0, maxVelocity);
            _animator.SetFloat(_velocityHash, _velocity);
        }
    }
}