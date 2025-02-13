using _1_Game.Systems.Character;
using Script.GameData;
using UnityEngine;
using UnityEngine.AI;

namespace _1_Game.Scripts.Systems.AIBehaviourTree
{
    public class MoveToPlayerNode : Node
    {
        public struct MoveParams
        {
            public Character Character;
            public NavMeshAgent Agent;
            public Transform Player;
            public CharacterDataConfig CharacterDataConfig;
        }

        private NavMeshAgent _agent;
        private Transform _player;
        private float _stoppingDistance;
        private MoveParams _moveParams;

        public MoveToPlayerNode(MoveParams moveParams)
        {
            _moveParams = moveParams;
            _agent = moveParams.Agent;
            _player = moveParams.Player;
            _stoppingDistance = moveParams.CharacterDataConfig.AttackRange;
            _agent.speed = moveParams.CharacterDataConfig.MoveSpeed;
        }

        public override NodeState Evaluate()
        {
            
            if (Vector3.Distance(_agent.transform.position, _player.position) <= _stoppingDistance)
            {
                _agent.ResetPath(); // Stop moving when close enough
                ExecuteAnimation(Vector3.zero);
                return NodeState.Success;
            }

            _agent.SetDestination(_player.position); // Move towards player
            if (_agent.velocity.magnitude > 0)
            {
                ExecuteAnimation(_agent.velocity);
            }
            return NodeState.Running;
        }

        private void ExecuteAnimation(Vector3 velocity)
        {
            CharacterAnimationController.MovementParameters moveParam = default;
            moveParam.Movement = velocity;
            moveParam.IsAiming = false;
            moveParam.AimingTarget = null;
            _moveParams.Character.Execute_MovementAnimation(moveParam);
        }
    }
}