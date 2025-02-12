
using Script.GameData.Weapon;
using UnityEngine;

namespace _1_Game.Scripts.Systems.WeaponSystem
{
    public class GunActor : IRangeActor
    {
        public void Attack(Transform actor, Vector3 targetDir, WeaponDataSet weaponDataSet)
        {
            Log.Debug("Gun actor attack");
            var rb = actor.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = false;
            rb.AddForce(targetDir * weaponDataSet.range, ForceMode.Impulse);
        }
    }
}