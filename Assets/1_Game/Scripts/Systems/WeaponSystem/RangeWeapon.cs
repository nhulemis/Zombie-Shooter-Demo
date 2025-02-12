using _1_Game.Scripts.Systems.AddressableSystem;
using UnityEngine;

namespace _1_Game.Scripts.Systems.WeaponSystem
{
    public class RangeWeapon : Weapon
    {
        [SerializeReference] private IRangeActor _actor;
        
        public override async void Attack(Vector3 targetDirection)
        {
            Log.Debug("Range weapon attack");
            if (WeaponDataSet.isSelfAttack) // the self attack
            {
                _actor.Attack(transform, targetDirection, WeaponDataSet);
            }
            else
            {
                var projectilePrefab = await AssetLoader.Load<GameObject>(WeaponDataSet.projectilePrefab);
                var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                projectile.transform.forward = targetDirection;
                _actor.Attack(projectile.transform, targetDirection, WeaponDataSet);
            }
        }
    }
}