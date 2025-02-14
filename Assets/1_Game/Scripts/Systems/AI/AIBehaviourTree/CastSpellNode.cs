using System;
using _1_Game.Scripts.DataConfig;
using _1_Game.Scripts.Util;
using _1_Game.Systems.Character;
using Cysharp.Threading.Tasks;
using Script.GameData;
using UnityEngine;

namespace _1_Game.Scripts.Systems.AIBehaviourTree
{
    public class CastSpellNode : Node
    {
        private CharacterActor _actor;
        private Transform _target;
        
        private bool _isCasting = false;
        private string _spellId;
        
        private float lastTimeCast = 0f;

        private GameDataBase GameDataBase => Locator<GameDataBase>.Get();
        private SpellConfig SpellConfig => GameDataBase.Get<SpellConfig>();

        public CastSpellNode(string spellId,  CharacterActor actor, Transform target)
        {
            _actor = actor;
            _target = target;
            _spellId = spellId;
        }
        
        public override NodeState Evaluate()
        {
            if (_isCasting)
            {
                return NodeState.Running;
            }
            return EvaluateCastSpell();
        }
        
        private NodeState EvaluateCastSpell()
        {
            if (string.IsNullOrEmpty(_spellId))
            {
                return NodeState.Success;
            }
            var spell = SpellConfig.GetSpellDataSet(_spellId);
            if (Time.time < lastTimeCast + spell.cooldown)
            {
                return NodeState.Success;
            }
            lastTimeCast = Time.time;
            _isCasting = true;
            CastSpell();
            return NodeState.Running;
        }

        private async void CastSpell()
        {
            var spell = SpellConfig.GetSpellDataSet(_spellId);
            _actor.CastSpell(spell, _target);
            await UniTask.Delay(TimeSpan.FromSeconds(spell.castTime));
            _isCasting = false;
        }
    }
}