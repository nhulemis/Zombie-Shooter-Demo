using System;
using System.Collections;
using _1_Game.Scripts.DataConfig;
using _1_Game.Scripts.Util;
using Script.GameData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _1_Game.Scripts.Controllers.Character
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(CharacterAnimationController))]
    public class Character : MonoBehaviour
    {
        [SerializeField] protected CharacterAnimationController _animationController;
        [SerializeField] protected CharacterController _controller;
        [SerializeField, ValueDropdown("CharacterConfigID")] string _characterConfigID;

        public IEnumerable CharacterConfigID => IDGetter.GetCharacterConfig();
        
        protected CharacterDataConfig CharacterDataConfig;
        [SerializeField]
        protected float VerticalVelocity;
        
        private void Awake()
        {
            CharacterDataConfig = SafetyDatabase.SafetyDB.Get<CharacterConfig>().Get(_characterConfigID);
        }

        private void FixedUpdate()
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
        }
    }
}