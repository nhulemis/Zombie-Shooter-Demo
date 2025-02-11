using System;
using System.Collections;
using _1_Game.Scripts.Systems.Pickup;
using _1_Game.Scripts.Util;
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
        
        public virtual void Attack()
        {
            throw new System.NotImplementedException();
        }

        public void Pickup()
        {
        }

        public void Drop()
        {
        }
    }
}