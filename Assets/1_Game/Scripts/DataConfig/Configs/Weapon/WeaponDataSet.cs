using System;
using UnityEngine;

namespace Script.GameData.Weapon
{
    [Serializable]
    public class WeaponDataSet : BaseRecord
    {
        [Header("Weapon Stats")]
        public float damage;
        public float attackRate;
        public float range;
        public Vector3 equipedOffsetPosition;
        public Quaternion equipedOffsetRotation;
    }
}