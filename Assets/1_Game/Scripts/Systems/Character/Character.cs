using System;
using System.Collections;
using _1_Game.Scripts.DataConfig;
using _1_Game.Scripts.Systems.WeaponSystem;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using Script.GameData;
using Script.GameData.Weapon;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _1_Game.Systems.Character
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(CharacterAnimationController))]
    [RequireComponent(typeof(WeaponController))]
    public class Character : MonoBehaviour
    {
        [SerializeField] protected CharacterAnimationController _animationController;
        [SerializeField] protected CharacterController _controller;
        [SerializeField] protected WeaponController _weaponController;
        [SerializeField, ValueDropdown("CharacterConfigID")] string _characterConfigID;

        public IEnumerable CharacterConfigID => IDGetter.GetCharacterConfigs();
        public CharacterAnimationController AnimationController => _animationController;
        
        public Weapon Weapon => _weaponController.EquippedWeapon;
        
        protected CharacterDataConfig CharacterDataConfig;
        protected float VerticalVelocity;
        protected bool isAiming;

        private void Awake()
        {
            CharacterDataConfig = SafetyDatabase.SafetyDB.Get<CharacterConfig>().Get(_characterConfigID);
            _weaponController.Init(this);
            _animationController.Init(CharacterDataConfig);
        }

        public virtual void Attack()
        {
        }

        protected float VerticalMovement()
        {
            if (_controller.isGrounded)
            {
                VerticalVelocity = -1;
            }
            else
            {
                VerticalVelocity -= Physics.gravity.magnitude * (Time.deltaTime * CharacterDataConfig.Mass);
            }

            return VerticalVelocity;
        }

        private void OnValidate()
        {
            if (_animationController == null)
            {
                _animationController = GetComponent<CharacterAnimationController>();
            }
            
            if(_controller == null)
            {
                _controller = GetComponent<CharacterController>();
            }
            
            if(_weaponController == null)
            {
                _weaponController = GetComponent<WeaponController>();
            }
        }
        
        protected void OverrideCharacterConfig(string characterConfigID)
        {
            var overrideData = SafetyDatabase.SafetyDB.Get<CharacterConfig>().Get(characterConfigID);
            if(overrideData == null) return;
            CharacterDataConfig = overrideData;
            _animationController.ApplyOverrideConfig(CharacterDataConfig);
            Log.Debug("[Character] Character Config Overridden: " + characterConfigID);
        }
        
        public async void ExecuteGrenade(Weapon grenade)
        {
            _weaponController.SwitchWeaponToIdleHand();
            _weaponController.EquipWeapon(grenade);
            _animationController.EquipWeapon(grenade);
            OverrideCharacterConfig(grenade.WeaponDataSet.overrideCharacterDataConfig);
            var grenadeThrow = _weaponController.EquippedWeapon;
            float timeToPlayAnim = _animationController.Execute_GrenadeThrow(grenadeThrow);
            await UniTask.Delay(TimeSpan.FromSeconds(timeToPlayAnim / 6));
            _weaponController.ThrowGrenade();
            _animationController.EquipWeapon(_weaponController.EquippedWeapon);
            OverrideCharacterConfig(_weaponController.EquippedWeapon.WeaponDataSet.overrideCharacterDataConfig);

        }
        
        public void Execute_MovementAnimation(CharacterAnimationController.MovementParameters parameters)
        {
            _animationController.Execute_MovementAnimation(parameters);
        }
        
        public void TakeDamage(float damage)
        {
            Log.Debug($"[Character] {name} took {damage} damage");
        }
        
        public void PickupWeapon(Weapon weapon)
        {
            OverrideCharacterConfig(weapon.WeaponDataSet.overrideCharacterDataConfig);
            _weaponController.EquipWeapon(weapon);
            _animationController.EquipWeapon(weapon);
        }

    }
}