using System;
using System.Collections;
using _1_Game.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Script.GameData.Weapon
{
    [Serializable]
    public class WeaponDataSet : BaseRecord
    {
        [Header("Weapon Stats")]
        public float damage;
        public float attackRate;
        public float range;
        public bool isAoEAttack;
        [ShowIf("isAoEAttack")]
        public float aoeRadius = 2;
        [ShowIf("isAoEAttack")]
        public float attackTime = 0.5f;
        public Vector3 equipedOffsetPosition;
        public Quaternion equipedOffsetRotation;
        
        [ValueDropdown("OverrideCharacterDataConfig")]
        public string overrideCharacterDataConfig;
        
        [Header("Projectile")]
        public bool isSelfAttack;
        [HideIf("isSelfAttack")]
        public AssetReference projectilePrefab;
        
        [Header("Combo Attack")]
        public bool hasAttack;
        [ShowIf("hasAttack")] public ComboAttackData[] comboAttackData;
        
        private IEnumerable OverrideCharacterDataConfig => IDGetter.GetCharacterConfigs();

        public ComboAttackData GetCombo(int i)
        {
            return comboAttackData[i];
        }
    }

    [Serializable]
    public class ComboAttackData
    {
        public string comboName;
        [ValueDropdown("parameterNames")]
        public string animationName;
        public int comboLevel;
        [SerializeReference]
        public ComboAttackData nextCombo;
        
        private IEnumerable parameterNames => IDGetter.GetParameterAnimationNames();
    }
}