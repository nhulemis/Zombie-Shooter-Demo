using System;
using _1_Game.Systems.Character;
using Script.GameData.Weapon;
using UnityEngine;

namespace _1_Game.Scripts.Systems.WeaponSystem
{
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class DamageRangeActor : MonoBehaviour
    {
        private float _damage;
        private CharacterActor _owner;
        private WeaponDataSet _weaponDataSet;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CharacterActor damageable))
            {
                if(_owner == null ) return;
                if(_owner.GetType() == damageable.GetType()) return;
                damageable.TakeDamage(_damage);
                
                if (_weaponDataSet.hasDamageEffect)
                {
                    damageable.ApplyDamageEffect(_weaponDataSet.damageEffect);
                }
            }
        }

        public void Init(WeaponDataSet weaponDataSet, CharacterActor owner)
        {
            Log.Debug("DamageRangeActor Init");
            _owner = owner;
            _weaponDataSet = weaponDataSet;
            var sphereCollider = GetComponent<SphereCollider>();
            sphereCollider.isTrigger = true;
            sphereCollider.radius = weaponDataSet.aoeRadius;
            GetComponent<Rigidbody>().isKinematic = true;
            _damage = weaponDataSet.damage;
            Destroy(gameObject, weaponDataSet.attackTime);
        }
    }
}