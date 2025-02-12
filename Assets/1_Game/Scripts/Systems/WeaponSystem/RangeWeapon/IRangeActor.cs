
using Script.GameData.Weapon;
using UnityEngine;

namespace _1_Game.Scripts.Systems.WeaponSystem
{
    public interface IRangeActor
    {
        void Attack(Transform actor, Vector3 targetDir, WeaponDataSet weaponDataSet);
    }
}