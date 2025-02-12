using System;
using System.Collections;
using _1_Game.Scripts.Systems.Pickup;
using _1_Game.Scripts.Systems.WeaponSystem.Commands;
using _1_Game.Scripts.Util;
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
        
        public virtual void Attack(Vector3 targetDirection)
        {
            Log.Debug("[Base] Weapon attack");
        }

        public async UniTask Pickup()
        {
            await new PickupWeaponCommand(this).Execute();
        }

        public void Drop()
        {
        }
    }
}