
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
            float throwForce = attackRange * 1.5f; 
            float upwardForce = attackRange / 3f; 

            Rigidbody rb = actor.GetComponent<Rigidbody>();
            if (rb == null)
            {
                Debug.LogError("No Rigidbody found on actor!");
                return;
            }

            rb.isKinematic = false; 
            rb.useGravity = true;

            Vector3 throwDirection = targetDir.normalized * throwForce + Vector3.up * upwardForce;
            rb.AddForce(throwDirection, ForceMode.Impulse);

            Debug.Log("Grenade attack launched with physics!");
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
            
            Log.Debug("Grenade attack");
            actor.DOPath(new []{startPos,peakPos,endPos}, throwDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                });
        }

        private void Explode()
        {
        }
    }
}