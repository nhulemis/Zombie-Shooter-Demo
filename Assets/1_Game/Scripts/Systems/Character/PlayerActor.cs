using System;
using _1_Game.Scripts.GamePlay;
using _1_Game.Scripts.Systems.InputSystem;
using _1_Game.Scripts.Systems.WeaponSystem;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace _1_Game.Systems.Character
{
    public class PlayerActor : CharacterActor
    {
        [SerializeField] private bool _isPlayer;

        [EnableIf("_isPlayer"), SerializeReference]
        private IPlayerInput _input;
        private Transform _aimTarget;
        private AutoAimingActorComponent _autoAimingActorComponent;

        private void Start()
        {
            _input.Initialize();
            _aimTarget = new GameObject("AimTarget").transform;
            _autoAimingActorComponent = _aimTarget.AddComponent<AutoAimingActorComponent>();
            _autoAimingActorComponent.Init(this);
            Locator<MapProvider>.Get().Init(this);
        }

        private void OnDestroy()
        {
            Destroy(_aimTarget.gameObject);
        }

        private void FixedUpdate()
        {
            if(IsStunned) return;
            if (!_isPlayer) return;
            if(this.IsUnityNull()) return;
            Movement();
            Attack();
            isAiming = _autoAimingActorComponent.IsAiming;
        }

        public override void Attack()
        {
            if (_input.IsAttacking())
            {
                _weaponController.AttackBy( _aimTarget.position);
            }
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

            CharacterAnimationController.MovementParameters movementParameters = default;
            movementParameters.Movement = movement;
            movementParameters.IsAiming = isAiming;
            movementParameters.AimingTarget = _aimTarget;
            movementParameters.IsEquippingWeapon = _weaponController.IsEquippedWeapon;
            Execute_MovementAnimation(movementParameters);
        }


        private void OnCollisionEnter(Collision other)
        {
            Log.Debug("Player collided with: " + other.gameObject.name);
        }

        private void OnTriggerEnter(Collider other)
        {
            Log.Debug("Player triggered with: " + other.gameObject.name);
        }

        private void OnDrawGizmos()
        {
            if (_aimTarget != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(_aimTarget.position, 0.5f);
            }
        }

        public void PutIntoInventory(WeaponActorComponent weapon)
        {
            _weaponController.PutIntoInventory(weapon);
        }

        public void SwapWeapon()
        {
            var nextWeapon = _weaponController.GetNextWeapon();
            PickupWeapon(nextWeapon, false);
        }
    }
}