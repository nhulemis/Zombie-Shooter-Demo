using System;
using System.Collections;
using _1_Game.Scripts.Systems.WeaponSystem;
using Sirenix.OdinInspector;

namespace UnityEngine.SceneManagement
{
    public class AttachComponent : MonoBehaviour
    {
        [SerializeField] private Transform _body;
        [SerializeField, ValueDropdown("BoneIds")] private Transform _bindToBone;
        [SerializeField] private AttachComponent _idleHand; // it can be another hand that is implemented AttackComponent

        private WeaponActorComponent equippedWeapon;
        public bool IsEquippedWeapon => equippedWeapon != null;
        public WeaponActorComponent EquippedWeapon => equippedWeapon;

        private IEnumerable BoneIds()
        {
            if(_body == null) return new ValueDropdownList<Transform>(){{"body is Null", null}};
            
            var bones = _body.GetComponentsInChildren<Transform>();
            var boneList = new ValueDropdownList<Transform>();
            foreach (var bone in bones)
            {
                boneList.Add(bone.name, bone);
            }
            return boneList;
        }

        private void Update()
        {
            if (_bindToBone == null) return;
            transform.position = _bindToBone.position;
            transform.rotation = _bindToBone.rotation;
        }
        
        public void AttachWeapon(WeaponActorComponent weapon, bool isKeepPosition = false)
        {
            if (weapon == null) return;

            equippedWeapon = weapon;
            weapon.transform.SetParent(transform); 
            if (isKeepPosition) return;
            weapon.transform.localPosition = weapon.WeaponDataSet.equipedOffsetPosition;
            weapon.transform.localRotation = weapon.WeaponDataSet.equipedOffsetRotation;
        }

        public WeaponActorComponent DetachWeapon()
        {
            if (!IsEquippedWeapon) return null;

            WeaponActorComponent weapon = equippedWeapon;
            equippedWeapon = null;
            weapon.transform.SetParent(null); 
            return weapon;
        }

        public void SwitchWeaponToIdleHand()
        {
            if (!IsEquippedWeapon || _idleHand == null) return;

            WeaponActorComponent weapon = DetachWeapon();
            weapon.gameObject.SetActive(false);
            _idleHand.AttachWeapon(weapon , true);
        }

        public void RetrieveWeaponFromIdleHand()
        {
            if (_idleHand == null || !_idleHand.IsEquippedWeapon) return;

            var currentWeapon = DetachWeapon();
            if (currentWeapon != null)
            {
                currentWeapon.gameObject.SetActive(false);
                currentWeapon.transform.SetParent(null);
                Destroy(currentWeapon.gameObject, 0.5f);
            }
            WeaponActorComponent weapon = _idleHand.DetachWeapon();
            weapon.gameObject.SetActive(true);
            AttachWeapon(weapon);
        }
    }
}