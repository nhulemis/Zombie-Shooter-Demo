using System;
using _1_Game.Systems.Character;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace _1_Game.Scripts.Systems.WeaponSystem
{
    public class WeaponController : MonoBehaviour
    {
        [FormerlySerializedAs("_attachComponent")] [SerializeField] private AttachComponent _rightHandAttachComponent;
        [SerializeField] private Weapon[] _weapons;
        public bool IsEquippedWeapon => _rightHandAttachComponent.IsEquippedWeapon;
        
        public Weapon EquippedWeapon => _rightHandAttachComponent.EquippedWeapon;
        private Character _actor;

        private void OnValidate()
        {
            if (_rightHandAttachComponent == null)
            {
                _rightHandAttachComponent = GetComponentInChildren<AttachComponent>();
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            if (weapon == null) return;
            var wpInstance = Instantiate(weapon);
            wpInstance.Init(_actor);
            _rightHandAttachComponent.AttachWeapon(wpInstance);
        }

        public void SwitchWeaponToIdleHand()
        {
            _rightHandAttachComponent.SwitchWeaponToIdleHand();
        }
        
        public void RetrieveWeaponFromIdleHand()
        {
            _rightHandAttachComponent.RetrieveWeaponFromIdleHand();
        }
        
        public void ThrowGrenade()
        {
            if (!IsEquippedWeapon) return;
            var weapon = _rightHandAttachComponent.DetachWeapon();
            weapon.Attack(transform.forward);
            RetrieveWeaponFromIdleHand();
        }

        public void Attack( Vector3 aimTargetPosition)
        {
            var targetDirection = aimTargetPosition - transform.position;
            EquippedWeapon?.Attack(targetDirection);
        }

        public void Init(Character character)
        {
            Log.Debug("WeaponController Init");
            _actor = character;
            if(EquippedWeapon != null)
                EquippedWeapon.Init(_actor);
        }
    }
}