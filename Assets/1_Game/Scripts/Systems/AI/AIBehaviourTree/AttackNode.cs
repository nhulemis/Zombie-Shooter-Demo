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

        private List<Node> _children;
        private float lastTimeCastSpell = 0f;

        GameDataBase GameDataBase => Locator<GameDataBase>.Get();
        SpellConfig SpellConfig => GameDataBase.Get<SpellConfig>();

        public AttackNode(CharacterActor actor, Transform target)
        {
            _actor = actor;
            _children = new List<Node>();
            if (!_actor.CharacterDataConfig.HasSpell) return;
            foreach (var spellId in _actor.CharacterDataConfig.SpellIds)
            {
                _children.Add(new CastSpellNode(spellId, _actor, target));
            }
        }

        public override NodeState Evaluate()
        {
            if (Time.time >= _lastAttackTime + GetAttackRate())
            {
                _lastAttackTime = Time.time;
                return EvaluateAttack();
            }

            return NodeState.Running;
        }

        private NodeState EvaluateAttack()
        {
            if (_children.Count == 0 || !CanAttackSpell())
            {
                _actor.Attack();
                return NodeState.Success;
            }
            
            lastTimeCastSpell = Time.time;
            bool isAnySpellRunning = false;
            foreach (var child in _children)
            {
                var result = child.Evaluate();
                if (result == NodeState.Running)
                {
                    isAnySpellRunning = true;
                    break;
                }
            }

            if (isAnySpellRunning)
            {
                return NodeState.Running;
            }

            _actor.Attack();
            return NodeState.Success;
        }
        
        private bool CanAttackSpell()
        {
            return Time.time >= lastTimeCastSpell + _actor.CharacterDataConfig.CountDownTime;
        }

        private float GetAttackRate()
        {
            return _actor.Weapon.GetAttackRate();
        }
    }
}