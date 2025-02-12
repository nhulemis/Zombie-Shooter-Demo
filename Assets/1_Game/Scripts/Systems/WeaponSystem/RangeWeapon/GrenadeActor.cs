
using DG.Tweening;
using Script.GameData.Weapon;
using UnityEngine;

namespace _1_Game.Scripts.Systems.WeaponSystem
{
    public class GrenadeActor : IRangeActor
    {
        public void Attack(Transform actor, Vector3 targetDir, WeaponDataSet weaponDataSet)
        {
            float attackRange = weaponDataSet.range;
            float throwDuration = weaponDataSet.attackRate;
            Vector3 startPos = actor.position;
            
            Vector3 endPos = startPos + targetDir * attackRange;
            endPos.y  = 0.3f;
            float height = attackRange / 3f; 
            Vector3 peakPos = (startPos + endPos) / 2f + Vector3.up * height;
            
            var rb = actor.GetComponent<Rigidbody>();
            
            
            actor.DOMove(peakPos, throwDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    rb.AddForce(targetDir * attackRange * 1.5f + Vector3.down * attackRange/2, ForceMode.Impulse);
                });
            
            // actor.DOPath(new Vector3[] { peakPos, endPos }, throwDuration, PathType.CatmullRom)
            //     .SetEase(Ease.Linear) 
            //     .OnComplete(() =>
            //     {
            //         Explode();
            //     });
        }
        
        private void Explode()
        {
            Log.Debug("Grenade exploded");
        }
    }
}