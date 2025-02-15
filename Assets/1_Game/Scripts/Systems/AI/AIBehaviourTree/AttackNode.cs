using System.Collections.Generic;
using _1_Game.Scripts.DataConfig;
using _1_Game.Scripts.Systems.Observe;
using _1_Game.Scripts.Util;
using _1_Game.Systems.Character;
using Cysharp.Threading.Tasks;
using Script.GameData;
using UniRx;
using Unity.VisualScripting;
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
        
        private float _mutiplier = 1f;
        private Transform _player;

        public AttackNode(CharacterActor actor, Transform target)
        {
            _actor = actor;
            _player = target;
            Locator<DoorObserver>.Get().RxIsDoorOpen.Subscribe(b =>
            {
                if (b)
                {
                    _mutiplier /= 2;
                }
            }).AddTo(_actor);
            if(Locator<DoorObserver>.Get().RxIsDoorOpen.Value)
            {
                _mutiplier /= 2;
            }
        }

        public override NodeState Evaluate()
        {
            if (Time.time >= _lastAttackTime + GetAttackRate())
            {
                _lastAttackTime = Time.time;
                RotateTowardsPlayer().Forget();
                return NodeState.Success;
            }
            return NodeState.Running;
        }
        
        private async UniTask RotateTowardsPlayer()
        {
            Vector3 direction = _player.position - _actor.transform.position;
            direction.y = 0; // Ignore vertical rotation

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            ExecuteAnimation(Vector3.zero);
            while (!_actor.IsUnityNull() && Quaternion.Angle(_actor.transform.rotation, targetRotation) > 2f)
            {
                
                _actor.transform.rotation = Quaternion.Slerp(
                    _actor.transform.rotation, 
                    targetRotation, 
                    Time.deltaTime * 15f
                );
        
                await UniTask.Yield();
            }
            if(_actor.IsUnityNull()) return;
            _actor.Attack();
        }
        
        private void ExecuteAnimation(Vector3 velocity)
        {
            CharacterAnimationController.MovementParameters moveParam = default;
            moveParam.Movement = velocity;
            moveParam.IsAiming = false;
            moveParam.AimingTarget = null;
            _actor.Execute_MovementAnimation(moveParam);
        }

        private bool CanAttackSpell()
        {
            return Time.time >= lastTimeCastSpell + _actor.CharacterDataConfig.CountDownSpellTime;
        }

        private float GetAttackRate()
        {
            return _actor.Weapon.GetAttackRate() * _mutiplier;
        }
    }
}