using _1_Game.Scripts.Systems.AddressableSystem;
using UnityEngine;

namespace _1_Game.Scripts.Systems.WeaponSystem
{
    public class RangeWeapon : Weapon
    {
        [SerializeReference] private IRangeActor _actor;
        [SerializeField] Transform _firePoint;
        
        private bool _isReadyToAttack = true;
        private float _lastAttackTime = 0;
        
        
        public override async void Attack(Vector3 targetDirection)
        {
            if (!_isReadyToAttack )
            {
                if (Time.time - _lastAttackTime > WeaponDataSet.attackRate)
                {
                    _isReadyToAttack = true;
                    _lastAttackTime = Time.time;
                }
                return;
            }
            Log.Debug("Range weapon attack");
            if (WeaponDataSet.isSelfAttack) // the self attack
            {
                _actor.Attack(transform, targetDirection, WeaponDataSet);
            }
            else
            {
                var projectilePrefab = await AssetLoader.Load<GameObject>(WeaponDataSet.projectilePrefab);
                var projectile = Instantiate(projectilePrefab, _firePoint.position, Quaternion.identity);
                projectile.transform.forward = _firePoint.forward;
                _actor.Attack(projectile.transform, _firePoint.forward, WeaponDataSet);
            }
            _isReadyToAttack = false;
        }
    }
}