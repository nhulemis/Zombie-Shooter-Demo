using UnityEngine;

namespace _1_Game.Scripts.Systems.WeaponSystem
{
    public class RangeWeapon : Weapon
    {
        [SerializeReference] private IRangeActor _actor;
        public override void Attack()
        {
            Log.Debug("Range weapon attack");
            _actor.Attack(Vector3.zero);
        }
    }
}