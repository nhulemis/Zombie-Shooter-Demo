using UnityEngine;

namespace _1_Game.Scripts.Systems.WeaponSystem
{
    public class MeleeWeapon : Weapon
    {
        public override void Attack(Vector3 targetDirection)
        {
            Log.Debug("Melee weapon attack");
        }
    }
}