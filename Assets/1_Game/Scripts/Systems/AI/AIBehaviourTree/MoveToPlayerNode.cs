using System;
using _1_Game.Scripts.Systems.Observe;
using _1_Game.Scripts.Util;
using _1_Game.Systems.Character;
using Cysharp.Threading.Tasks;
using Script.GameData;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace _1_Game.Scripts.Systems.AIBehaviourTree
{
    public class MoveToPlayerNode : Node
    {
        public struct MoveParams
        {
            public CharacterActor Acttor;
            public NavMeshAgent Agent;
            public CharacterActor Player;
        }

        private NavMeshAgent _agent;
        private Transform _player;
        private float _stoppingDistance;
        private MoveParams _moveParams;

        public MoveToPlayerNode(MoveParams moveParams)
        {
            _moveParams = moveParams;
            _agent = moveParams.Agent;
            _player = moveParams.Player.transform;
            _stoppingDistance = moveParams.Acttor.Weapon.WeaponDataSet.range;
            _agent.speed = moveParams.Acttor.CharacterDataConfig.MoveSpeed;

            if(Locator<DoorObserver>.Get().RxIsDoorOpen.Value)
            {
                _agent.speed *= 1.5f;
            }
            
            RegisterListener();
        }

        private void RegisterListener()
        {
            _moveParams.Acttor.RxIsStunned.Subscribe(isStunned =>
            {
                if (isStunned)
                {
                    _agent.ResetPath();
                }
            }).AddTo(_agent);
            Locator<DoorObserver>.Get().RxIsDoorOpen.Subscribe(b =>
            {
                if (b)
                {
                    _agent.speed *= 2;
                }
            }).AddTo(_agent);
        }

        public override NodeState Evaluate()
        {
            if(!_agent.isOnNavMesh)
            {
                return NodeState.Failure;
            }
            float distance = Vector3.Distance(_agent.transform.position, _player.position);
            if (distance <= _stoppingDistance)
            {
                _agent.ResetPath(); // Stop moving when close enough
                ExecuteAnimation(Vector3.zero);
                return NodeState.Success;
            }
            var destination = _player.position - (_player.position - _agent.transform.position).normalized * _stoppingDistance;
            _agent.SetDestination(destination); // Move towards player
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
            _moveParams.Acttor.Execute_MovementAnimation(moveParam);
        }
    }
}