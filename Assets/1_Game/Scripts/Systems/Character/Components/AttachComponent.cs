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

        private Weapon equippedWeapon;

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

        public void EquipWeapon(Weapon weapon)
        {
            if(equippedWeapon != null) UnequipWeapon();
            if(weapon == null) return;
            equippedWeapon = Instantiate(weapon, transform);
            equippedWeapon.transform.localPosition = weapon.WeaponDataSet.equipedOffsetPosition;
            equippedWeapon.transform.localRotation = weapon.WeaponDataSet.equipedOffsetRotation;
        }
        
        public void UnequipWeapon()
        {
            if(equippedWeapon == null) return;
            
            Destroy(equippedWeapon.gameObject);
        }

        private void Update()
        {
            if (_bindToBone == null) return;
            transform.position = _bindToBone.position;
            transform.rotation = _bindToBone.rotation;
        }
    }
}