using _1_Game.Systems.Character;
using UnityEngine;

namespace _1_Game.Scripts.Systems.AIBehaviourTree
{
    public class AttackNode : Node
    {
        private float _lastAttackTime = 0f;
        private Character _actor;

        public AttackNode(Character actor)
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
        
        private float GetAttackRate()
        {
            return _actor.Weapon.GetAttackRate();
        }
    }

}