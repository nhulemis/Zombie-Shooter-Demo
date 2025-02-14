
using DG.Tweening;
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

        public void AttackTo(Transform actor, Vector3 endPos, WeaponDataSet weaponDataSet)
        {
            float attackRange = weaponDataSet.range;
            float throwDuration = 0.25f;
            Vector3 startPos = actor.position;
            
            endPos.y  = 0.3f;
            float height = attackRange / 3f; 
            Vector3 peakPos = (startPos + endPos) / 2f + Vector3.up * height;
            
            var rb = actor.GetComponent<Rigidbody>();
            
            Log.Debug("Gun attack");
            actor.DOPath(new []{startPos,endPos}, throwDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                });
        }
    }
}