using System;
using System.Collections.Generic;
using _1_Game.Scripts.Systems.AddressableSystem;
using _1_Game.Systems.Character;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace _1_Game.Scripts.Systems.WeaponSystem
{
    public class WeaponController : MonoBehaviour
    {
        [FormerlySerializedAs("_attachComponent")] [SerializeField] private AttachComponent _rightHandAttachComponent;
        [SerializeField] private List<WeaponActorComponent> _weapons;
        public bool IsEquippedWeapon => _rightHandAttachComponent.IsEquippedWeapon;
        
        public WeaponActorComponent EquippedWeapon => _rightHandAttachComponent.EquippedWeapon;
        private CharacterActor _actor;
        
        public List<WeaponActorComponent> Weapons => _weapons;
        
        public void Init(CharacterActor character)
        {
            Log.Debug("WeaponController Init");
            _actor = character;
            if(EquippedWeapon != null)
                EquippedWeapon.Init(_actor);
            
            if(_weapons.Count > 0)
                _actor.PickupWeapon(_weapons[0]);
        }

        private void OnValidate()
        {
            if (_rightHandAttachComponent == null)
            {
                _rightHandAttachComponent = GetComponentInChildren<AttachComponent>();
                if (_rightHandAttachComponent == null)
                {
                    var rightHand = new GameObject("RightHandAttachComponent");
                    _rightHandAttachComponent = rightHand.AddComponent<AttachComponent>();
                    rightHand.transform.SetParent(transform);
                }
            }
        }

        public void EquipWeapon(WeaponActorComponent weapon)
        {
            if (weapon == null) return;
            var wpInstance = Instantiate(weapon);
            wpInstance.gameObject.SetActive(true);
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
        }

        public void AttackBy(Vector3 direction)
        {
            var targetDirection = direction - transform.position;
            EquippedWeapon?.Attack(targetDirection);
        }


        public void AttackTo(Vector3 target)
        {
            EquippedWeapon?.AttackTo(target);
        }

        public async void PutIntoInventory(WeaponActorComponent weapon)
        {
            if(this.IsUnityNull()) return;
            if(Weapons.Exists(x=>x.Id.Equals(weapon.Id) )) return;
            var weaponInstance = await AssetLoader.Instantiate(weapon,Vector3.zero, quaternion.identity);
            weaponInstance.gameObject.SetActive(false);
            var attachComponent = weaponInstance.GetComponent<WeaponActorComponent>();
            Weapons.Add(attachComponent);
        }

        public WeaponActorComponent GetNextWeapon()
        {
            if (Weapons.Count == 0) return null;
            var index = Weapons.FindIndex(x =>x.Id.Equals(EquippedWeapon.Id));
            index++;
            if (index >= Weapons.Count)
            {
                index = 0;
            }

            return Weapons[index];
        }

        public void Drop()
        {
            var currentWeapon = _rightHandAttachComponent.DetachWeapon();
            if (currentWeapon != null)
            {
                currentWeapon.gameObject.SetActive(false);
                currentWeapon.transform.SetParent(null);
                Destroy(currentWeapon.gameObject, 0.5f);
            }
        }
    }
}