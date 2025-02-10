using System;
using _1_Game.Scripts.Systems.InputSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _1_Game.Scripts.Controllers.Character
{
    public class Player : Character
    {
        [SerializeField] private bool _isPlayer;

        [EnableIf("_isPlayer"), SerializeReference]
        private IPlayerInput _input;

        private void Start()
        {
            if (_isPlayer)
            {
                _animationController.EquipWeapon();
            }
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
        }
        

        private void OnCollisionEnter(Collision other)
        {
            Log.Debug("Player collided with: " + other.gameObject.name);
        }

        private void OnTriggerEnter(Collider other)
        {
            Log.Debug("Player triggered with: " + other.gameObject.name);
        }
    }
}