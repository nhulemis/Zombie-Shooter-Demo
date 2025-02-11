using System;
using System.Collections;
using _1_Game.Scripts.DataConfig;
using _1_Game.Scripts.Systems.WeaponSystem;
using _1_Game.Scripts.Util;
using Script.GameData;
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
        
        protected CharacterDataConfig CharacterDataConfig;
        protected float VerticalVelocity;
        protected bool isAiming;

        private void Awake()
        {
            CharacterDataConfig = SafetyDatabase.SafetyDB.Get<CharacterConfig>().Get(_characterConfigID);
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
            CharacterDataConfig = SafetyDatabase.SafetyDB.Get<CharacterConfig>().Get(characterConfigID);
            Log.Debug("[Character] Character Config Overridden: " + characterConfigID);
        }
    }
}