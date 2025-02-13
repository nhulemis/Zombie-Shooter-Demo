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

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CharacterActor damageable))
            {
                damageable.TakeDamage(_damage);
            }
        }

        public void Init(WeaponDataSet weaponDataSet)
        {
            var sphereCollider = GetComponent<SphereCollider>();
            sphereCollider.isTrigger = true;
            sphereCollider.radius = weaponDataSet.aoeRadius;
            GetComponent<Rigidbody>().isKinematic = true;
            _damage = weaponDataSet.damage;
            Destroy(gameObject, weaponDataSet.attackTime);
        }
    }
}