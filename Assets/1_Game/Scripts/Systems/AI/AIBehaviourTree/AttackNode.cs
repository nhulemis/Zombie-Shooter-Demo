using UnityEngine;

namespace _1_Game.Scripts.Systems.AIBehaviourTree
{
    public class AttackNode : Node
    {
        private Animator _animator;
        private float _attackCooldown = 1f;
        private float _lastAttackTime = 0f;

        public AttackNode(Animator animator)
        {
            this._animator = animator;
        }

        public override NodeState Evaluate()
        {
            if (Time.time >= _lastAttackTime + _attackCooldown)
            {
                _lastAttackTime = Time.time;
                _animator.SetTrigger("Attack");
                return NodeState.Success;
            }
            return NodeState.Running;
        }
    }

}