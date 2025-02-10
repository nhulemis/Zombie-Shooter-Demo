using System;
using UnityEngine;

namespace _1_Game.Scripts.Controllers.Character
{
    public class CharacterAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        private const string _layerAiming = "Aiming";

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
    }
}