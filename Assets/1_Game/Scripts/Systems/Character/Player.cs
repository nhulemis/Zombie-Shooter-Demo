using System;
using _1_Game.Scripts.Systems.InputSystem;
using _1_Game.Scripts.Systems.WeaponSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _1_Game.Systems.Character
{
    public class Player : Character
    {
        [SerializeField] private bool _isPlayer;

        [EnableIf("_isPlayer"), SerializeReference]
        private IPlayerInput _input;

        private void Start()
        {
            _input.Initialize();
        }

        private void FixedUpdate()
        {
            if (!_isPlayer) return;
            Movement();
        }

        private void Movement()
        {
            Vector3 movement = _input.GetMovement() * CharacterDataConfig.MoveSpeed;
            movement.y = VerticalMovement();
            _controller.Move(movement * Time.deltaTime);
            
            // Rotate player in movement direction
            if (movement.x != 0 || movement.z != 0) // Check if moving
            {
                Vector3 lookDirection = new Vector3(movement.x, 0, movement.z); // Ignore Y-axis
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * CharacterDataConfig.RotationSpeed);
            }
            _animationController.Execute_MovementAnimation(movement, _weaponController.IsAiming);
        }
        

        private void OnCollisionEnter(Collision other)
        {
            Log.Debug("Player collided with: " + other.gameObject.name);
        }

        private void OnTriggerEnter(Collider other)
        {
            Log.Debug("Player triggered with: " + other.gameObject.name);
        }

        public void PickupWeapon(Weapon weapon)
        {
            _weaponController.EquipWeapon(weapon);
            _animationController.EquipWeapon(weapon);
        }
    }
}