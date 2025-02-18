using System;
using System.Collections.Generic;
using _1_Game.Scripts.Systems.WeaponSystem;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using Script.GameData;
using Script.GameData.Weapon;
using Unity.VisualScripting;
using UnityEngine;

namespace _1_Game.Systems.Character
{
    public class CharacterAnimationController : MonoBehaviour
    {
        public struct MovementParameters
        {
            public Vector3 Movement;
            public bool IsAiming;
            public Transform AimingTarget;
            public bool IsEquippingWeapon;
        }

        [SerializeField] private Animator _animator;
        [SerializeField] private float acceleration = 0.1f;
        [SerializeField] private float deceleration = 0.5f;

        private readonly int _velocityHash = Animator.StringToHash("Velocity");
        private readonly int _speedHash = Animator.StringToHash("Speed");
        private readonly int _grenadeThrowHash = Animator.StringToHash("Grenade");
        private readonly int _comboHash = Animator.StringToHash("SkillCombo");

        private float _velocity;
        private AnimatorOverrideController _overrideController;
        private CharacterDataConfig _characterDataConfig;
        public Animator Animator => _animator;


        public void Init(CharacterDataConfig characterDataConfig)
        {
            _overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
            _animator.runtimeAnimatorController = _overrideController;
            ApplyOverrideConfig(characterDataConfig);
        }

        private void ReplaceAnimationClip(string originalClipName, AnimationClip newClip)
        {
            if (newClip == null)
            {
                Debug.LogError("New animation clip is missing!");
                return;
            }

            List<KeyValuePair<AnimationClip, AnimationClip>> overrides =
                new List<KeyValuePair<AnimationClip, AnimationClip>>();
            _overrideController.GetOverrides(overrides);

            for (int i = 0; i < overrides.Count; i++)
            {
                if (overrides[i].Key.name == originalClipName)
                {
                    overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[i].Key, newClip);
                    Log.Debug($"Replaced {originalClipName} with {newClip.name}");
                    break;
                }
            }

            _overrideController.ApplyOverrides(overrides);
        }

        private void OnValidate()
        {
            if (_animator == null)
            {
                _animator = GetComponentInChildren<Animator>();
            }
        }

        public void EquipWeapon(WeaponActorComponent weapon)
        {
            ResetWeightLayer();
            int layerAimingIndex = _animator.GetLayerIndex(weapon.PoseLayerName);
            _animator.SetLayerWeight(layerAimingIndex, 1);
        }

        private void ResetWeightLayer()
        {
            if(this.IsUnityNull()) return;
            if (_animator.layerCount <= 1) return;
            for (int i = 1; i < _animator.layerCount; i++)
            {
                _animator.SetLayerWeight(i, 0);
            }
        }

        public void Execute_MovementAnimation(MovementParameters parameters)
        {
            Vector3 movement = parameters.Movement;
            Transform aimingTarget = parameters.AimingTarget;

            bool isMoving = movement.x != 0 || movement.z != 0;

            if (isMoving && _velocity < 1)
            {
                _velocity += acceleration * Time.fixedDeltaTime;
            }
            else if (!isMoving && _velocity > 0)
            {
                _velocity -= deceleration * Time.fixedDeltaTime;
            }

            if (!isMoving && _velocity < 0)
            {
                _velocity = 0;
            }

            float maxVelocity = parameters.IsEquippingWeapon ? 0.5f : 1;
            _velocity = Mathf.Clamp(_velocity, 0, maxVelocity);
            _animator.SetFloat(_velocityHash, _velocity);

            float speed = 1f;
            if (parameters.IsAiming)
            {
                float movementDirectionSign = transform.GetMovementDirectionSign(movement, aimingTarget);
                speed = movementDirectionSign < 0 ? -1f : movementDirectionSign > 0 ? 1f : 0f;
            }

            _animator.SetFloat(_speedHash, speed);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="grenade"></param>
        /// <returns>Time play the aimation</returns>
        public float Execute_GrenadeThrow(WeaponActorComponent grenade)
        {
            _animator.SetTrigger(_grenadeThrowHash);
            int layerIndex = _animator.GetLayerIndex(grenade.PoseLayerName);
            var clipInfo = _animator.GetCurrentAnimatorClipInfo(layerIndex);
            float clipLength = clipInfo[0].clip.length;
            return clipLength;
        }

        public async UniTask<float> AttackByAnimation(ComboAttackData attackData, WeaponActorComponent weapon)
        {
            if(this.IsUnityNull()) return 0f;
            if (attackData == null)
            {
                _animator.SetInteger(_comboHash, 0);
                return 0;
            }

            int idHash = Animator.StringToHash(attackData.animationName);
            int comboLevel = attackData.comboLevel;
            _animator.SetInteger(idHash, comboLevel);
            await UniTask.NextFrame();
            int layerIndex = _animator.GetLayerIndex(weapon.PoseLayerName);
            var clipInfo = _animator.GetCurrentAnimatorClipInfo(layerIndex);
            float clipLength = clipInfo[0].clip.length;
            return clipLength;
        }

        public async void ApplyOverrideConfig(CharacterDataConfig characterDataConfig)
        {
            if(this.IsUnityNull()) return ;
            _characterDataConfig = characterDataConfig;
            if (characterDataConfig.OverrideClips.Count == 0) return;
            foreach (var clip in characterDataConfig.OverrideClips)
            {
                ReplaceAnimationClip(clip.MappingTo, clip.Clip);
            }
        }

        public async UniTask Execute_DeathAnimation()
        {
            await UniTask.Delay(500);
        }
    }

    public class AnimationClipOverrides : Dictionary<AnimationClip, AnimationClip>
    {
        public AnimationClipOverrides(int capacity) : base(new Dictionary<AnimationClip, AnimationClip>(capacity))
        {
        }
    }
}