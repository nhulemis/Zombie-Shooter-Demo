using System;
using System.Collections;
using _1_Game.Scripts.Systems.Pickup;
using _1_Game.Scripts.Systems.WeaponSystem.Commands;
using _1_Game.Scripts.Util;
using _1_Game.Systems.Character;
using Cysharp.Threading.Tasks;
using Script.GameData.Weapon;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _1_Game.Scripts.Systems.WeaponSystem
{
    [Serializable]
    public class Weapon : MonoBehaviour , IPickupableObject
    {
        [SerializeField]
        public WeaponDataSet WeaponDataSet;
        [field: SerializeField, ValueDropdown("animationLayerNames")] public string PoseLayerName { get; set; }
        public IEnumerable WeaponDataSetIds => IDGetter.GetWeaponConfigs();
        
        private IEnumerable animationLayerNames => IDGetter.GetAnimationLayerMappingConfigs();
        
        protected bool _isReadyToAttack = true;
        protected float _lastAttackTime = 0;
        protected Character _actor;

        public void Init(Character actor)
        {
            Log.Debug("[Base] Weapon init");
            _actor = actor;
        }
        
        public virtual void Attack( Vector3 targetDirection)
        {
            Log.Debug("[Base] Weapon attack");
            if (!_isReadyToAttack)
            {
                if (Time.time - _lastAttackTime > WeaponDataSet.attackRate)
                {
                    _isReadyToAttack = true;
                    _lastAttackTime = Time.time;
                }
            }
        }

        public virtual void Free()
        {
            _lastAttackTime = float.MaxValue;
        }

        public async UniTask Pickup()
        {
            await new PickupWeaponCommand(this).Execute();
        }

        public void Drop()
        {
        }

        public float GetAttackRate()
        {
            return WeaponDataSet.attackRate;
        }
    }
}