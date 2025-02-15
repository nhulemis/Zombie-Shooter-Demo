using System;
using System.Collections;
using System.Collections.Generic;
using _1_Game.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.GameData
{
    [Serializable]
    public class CharacterDataConfig : BaseRecord
    {
        public float Mass = 2f;
        public float MoveSpeed = 4f;
        public float JumpForce = 2f;
        public float RotationSpeed = 10f;
        public float AimOffsetAngle = 10f;
        public float AttackRange = 2f;
        public float Health = 50f;
        
        [Header("Animation")]
        [FoldoutGroup("Animation Override")]
        public List<OverrideClip> OverrideClips = new List<OverrideClip>();
        
        [Header("Spell")]
        public bool HasSpell;
        [FormerlySerializedAs("CountDownTime")]
        [FoldoutGroup("Spell")]
        [ ShowIf("HasSpell")]
        public float CountDownSpellTime = 10f;
        [FoldoutGroup("Spell")]
        [ValueDropdown("SpellIdsGetter"), ShowIf("HasSpell")]
        public List<string> SpellIds = new List<string>();
        
        private IEnumerable SpellIdsGetter => IDGetter.GetSpellDataSets();
        
    }
    
    [Serializable]
    public struct OverrideClip
    {
        [ValueDropdown("BaseClipNames")]
        public string MappingTo;
        public AnimationClip Clip;
        
        private IEnumerable BaseClipNames=> IDGetter.GetAnimationClips();
    }
}