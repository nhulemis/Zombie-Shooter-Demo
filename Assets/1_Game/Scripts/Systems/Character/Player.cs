using System;
using _1_Game.Scripts.Systems.InputSystem;
using _1_Game.Scripts.Systems.WeaponSystem;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace _1_Game.Systems.Character
{
    public class Player : Character
    {
        [SerializeField] private bool _isPlayer;

        [EnableIf("_isPlayer"), SerializeReference]
        private IPlayerInput _input;

        [ShowIf("_isPlayer"), SerializeField] private bool _isAutoAim;
        
        private Transform _aimTarget;

        private void Start()
        {
            _input.Initialize();
            _aimTarget = new GameObject("AimTarget").transform;
            _aimTarget.AddComponent<AimingToMouseActorComponent>();
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
            //if is backwards movement, reduce speed by 25%
            if(isAiming && transform.GetMovementDirectionSign(movement, _aimTarget) < 0)
            {
                movement *= 0.75f;
            }
            _controller.Move(movement * Time.deltaTime);

            // Prioritize aiming at _aimTarget if it exists
            if (_aimTarget != null && isAiming)
            {
                Vector3 aimDirection = _aimTarget.position - transform.position;
                aimDirection.y = 0; // Ignore vertical rotation

                if (aimDirection.sqrMagnitude > 0.01f) // Prevent tiny movements
                {
                    Quaternion targetRotation = Quaternion.LookRotation(aimDirection);
                    targetRotation *= Quaternion.Euler(0, CharacterDataConfig.AimOffsetAngle, 0);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * CharacterDataConfig.RotationSpeed);
                }
            }
            else if (movement.x != 0 || movement.z != 0) // If no aim target, rotate by movement
            {
                Vector3 moveDirection = new Vector3(movement.x, 0, movement.z);
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * CharacterDataConfig.RotationSpeed);
            }

            CharacterAnimationController.MovementParameters movementParameters;
            movementParameters.Movement = movement;
            movementParameters.IsAiming = isAiming;
            movementParameters.AimingTarget = _aimTarget;
            movementParameters.IsEquippingWeapon = _weaponController.IsEquippedWeapon;
            _animationController.Execute_MovementAnimation(movementParameters);
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
            OverrideCharacterConfig(weapon.WeaponDataSet.overrideCharacterDataConfig);
            _weaponController.EquipWeapon(weapon);
            _animationController.EquipWeapon(weapon);
        }

        public async void ExecuteGrenade(Weapon grenade)
        {
            _weaponController.SwitchWeaponToIdleHand();
            _weaponController.EquipWeapon(grenade);
            _animationController.EquipWeapon(grenade);
            var grenadeThrow = _weaponController.EquippedWeapon;
            float timeToPlayAnim = _animationController.Execute_GrenadeThrow(grenadeThrow);
            await UniTask.Delay(TimeSpan.FromSeconds(timeToPlayAnim / 6));
            _weaponController.ThrowGrenade();
            _animationController.EquipWeapon(_weaponController.EquippedWeapon);
        }
    }
}