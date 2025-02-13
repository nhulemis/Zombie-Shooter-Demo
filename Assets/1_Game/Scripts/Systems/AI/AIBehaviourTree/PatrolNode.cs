using System.Collections.Generic;
using _1_Game.Systems.Character;

namespace _1_Game.Scripts.Systems.AIBehaviourTree
{
    using UnityEngine;
    using UnityEngine.AI;

    public class PatrolNode : Node
    {
        private NavMeshAgent _agent;
        private List<Vector3> _patrolPoints;
        private int _currentPatrolIndex = 0;
        private Character _actor;

        public PatrolNode(Character actor,NavMeshAgent agent, List<Vector3> patrolPoints)
        {
            this._agent = agent;
            this._patrolPoints = patrolPoints;
            this._actor = actor;
        }

        public override NodeState Evaluate()
        {
            if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
            {
                _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Count;
                _agent.SetDestination(_patrolPoints[_currentPatrolIndex]);
            }
            
            _actor.Execute_MovementAnimation(new CharacterAnimationController.MovementParameters
            {
                Movement = _agent.velocity,
                IsAiming = false,
                AimingTarget = null,
                IsEquippingWeapon = false
            });
            return NodeState.Running;
        }
    }

}