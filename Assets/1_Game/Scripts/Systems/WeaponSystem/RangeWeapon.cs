using System;
using _1_Game.Scripts.Systems.AddressableSystem;
using _1_Game.Systems.Character;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace _1_Game.Scripts.Systems.WeaponSystem
{
    public class RangeWeapon : WeaponActorComponent
    {
        [SerializeReference] private IRangeActor _rangeActor;
        [SerializeField] Transform _firePoint;
        
        public Type GetRangeActorType => _rangeActor.GetType();
        
        private async void HandleAttack(Vector3 target, bool isDirectional)
        {
            if(this.IsUnityNull()) return;
            if (!_isReadyToAttack) return;
            _isReadyToAttack = false;
            _lastAttackTime = Time.time;

            if (WeaponDataSet.isSelfAttack) // the self attack
            {
                if (isDirectional)
                    _rangeActor.Attack(transform, target, WeaponDataSet);
                else
                    _rangeActor.AttackTo(transform, target, WeaponDataSet);

                if (TryGetComponent(out Projectile projectileComponent))
                {
                    projectileComponent.Init(WeaponDataSet, _actor);
                }
            }
            else
            {
                var projectilePrefab = await AssetLoader.Load<GameObject>(WeaponDataSet.projectilePrefab);
                if(projectilePrefab == null) return;
                var projectile = Instantiate(projectilePrefab, _firePoint.position, Quaternion.identity);
                projectile.transform.forward = _firePoint.forward;
                
                if (projectile.TryGetComponent(out Projectile projectileComponent))
                {
                    projectileComponent.Init(WeaponDataSet, _actor);
                }

                if (isDirectional)
                    _rangeActor.Attack(projectile.transform, _firePoint.forward, WeaponDataSet);
                else
                    _rangeActor.AttackTo(projectile.transform, target, WeaponDataSet);
            }
        }

        public override async void Attack(Vector3 targetDirection)
        {
            base.Attack(targetDirection);
            HandleAttack(targetDirection, true);
        }

        public override async void AttackTo(Vector3 target)
        {
            base.AttackTo(target);
            HandleAttack(target, false);
        }
    }
}