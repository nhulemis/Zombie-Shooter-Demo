using System;
using _1_Game.Systems.Character;
using Script.GameData.Weapon;
using UnityEngine;

namespace _1_Game.Scripts.Systems.WeaponSystem
{
    public class Projectile : MonoBehaviour, IProjectile
    {
        [SerializeField] GameObject _explosionPrefab;
        [SerializeField] bool _isAOE;

        private WeaponDataSet _weaponDataSet;
        private CharacterActor _owner;

        public void Init(WeaponDataSet weaponDataSet, CharacterActor owner)
        {
            _weaponDataSet = weaponDataSet;
            _owner = owner;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (_isAOE)
            {
                var damageRange = new GameObject("AOE").AddComponent<DamageRangeActor>();
                damageRange.Init(_weaponDataSet, _owner);
                damageRange.transform.position = transform.position;
            }
            else if (other.gameObject.TryGetComponent(out CharacterActor damageable))
            {
                if (_owner == null) return;
                if (_owner.GetType() == damageable.GetType()) return;
                damageable.TakeDamage(_weaponDataSet.damage);
                if (_weaponDataSet.hasDamageEffect)
                {
                    damageable.ApplyDamageEffect(_weaponDataSet.damageEffect);
                }
            }


            Explode();
        }

        protected void Explode()
        {
            Log.Debug("Projectile explode");
            var explosion = Instantiate(_explosionPrefab, transform.position + Vector3.up, Quaternion.identity);
            Destroy(explosion, 1f);

            Destroy(gameObject);
        }
    }
}