using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _1_Game.Scripts.Systems.WeaponSystem
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private AttachComponent _attachComponent;
        [SerializeField] private Weapon[] _weapons;
        public bool IsAiming => _attachComponent.IsEquippedWeapon;

        private void OnValidate()
        {
            if (_attachComponent == null)
            {
                _attachComponent = GetComponentInChildren<AttachComponent>();
            }
        }
        
        public void EquipWeapon(int weaponIndex)
        {
            if (weaponIndex < 0 || weaponIndex >= _weapons.Length) return;
            _attachComponent.EquipWeapon(_weapons[weaponIndex]);
        }

        public void EquipWeapon(Weapon weapon)
        {
            if (weapon == null) return;
            _attachComponent.EquipWeapon(weapon);
        }
    }
}