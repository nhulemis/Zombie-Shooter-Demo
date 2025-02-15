using System.Collections.Generic;
using _1_Game.Scripts.DataConfig;
using _1_Game.Scripts.Util;
using _1_Game.Systems.Character;
using Script.GameData;
using UnityEngine;

namespace _1_Game.Scripts.Systems.AIBehaviourTree
{
    public class AttackNode : Node
    {
        private float _lastAttackTime = 0f;
        private CharacterActor _actor;

        private float lastTimeCastSpell = 0f;

        GameDataBase GameDataBase => Locator<GameDataBase>.Get();
        SpellConfig SpellConfig => GameDataBase.Get<SpellConfig>();

        public AttackNode(CharacterActor actor, Transform target)
        {
            _actor = actor;
        }

        public override NodeState Evaluate()
        {
            if (Time.time >= _lastAttackTime + GetAttackRate())
            {
                _lastAttackTime = Time.time;
                _actor.Attack();
                return NodeState.Success;
            }
            return NodeState.Running;
        }

        private bool CanAttackSpell()
        {
            return Time.time >= lastTimeCastSpell + _actor.CharacterDataConfig.CountDownSpellTime;
        }

        private float GetAttackRate()
        {
            return _actor.Weapon.GetAttackRate();
        }
    }
}