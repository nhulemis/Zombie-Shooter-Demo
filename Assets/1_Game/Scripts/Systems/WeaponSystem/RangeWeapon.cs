using _1_Game.Scripts.Systems.AddressableSystem;
using _1_Game.Systems.Character;
using UnityEngine;
using UnityEngine.Serialization;

namespace _1_Game.Scripts.Systems.WeaponSystem
{
    public class RangeWeapon : Weapon
    {
        [SerializeReference] private IRangeActor _rangeActor;
        [SerializeField] Transform _firePoint;
        
        public override async void Attack( Vector3 targetDirection)
        {
            base.Attack( targetDirection);
            if (!_isReadyToAttack ) return;
            _isReadyToAttack = false;
            _lastAttackTime = Time.time;
            Log.Debug("Range weapon attack");
            if (WeaponDataSet.isSelfAttack) // the self attack
            {
                _rangeActor.Attack(transform, targetDirection, WeaponDataSet);
                if(TryGetComponent(out Projectile projectileComponent))
                {
                    projectileComponent.Init(WeaponDataSet, _actor);
                }
            }
            else
            {
                var projectilePrefab = await AssetLoader.Load<GameObject>(WeaponDataSet.projectilePrefab);
                var projectile = Instantiate(projectilePrefab, _firePoint.position, Quaternion.identity);
                projectile.transform.forward = _firePoint.forward;
                if(projectile.TryGetComponent(out Projectile projectileComponent))
                {
                    projectileComponent.Init(WeaponDataSet , _actor);
                }
                _rangeActor.Attack(projectile.transform, _firePoint.forward, WeaponDataSet);
            }
        }
    }
}