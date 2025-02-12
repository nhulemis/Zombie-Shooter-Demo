using System;
using System.Collections.Generic;
using System.Linq;
using _1_Game.Systems.Character;
using Cysharp.Threading.Tasks;
using Script.GameData.Weapon;
using UnityEngine;

namespace _1_Game.Scripts.Systems.WeaponSystem
{
    public class MeleeWeapon : Weapon
    {
        private ComboAttackData currentComboAttackData;
        
        private Queue<ComboAttackData> _comboAttackQueue = new (4);

        private bool _isPlayingComboAttack;
        public override void Attack(Vector3 targetDirection)
        {
            base.Attack(targetDirection);
            if(!_isReadyToAttack) return;
            _lastAttackTime = Time.time;
            _isReadyToAttack = false;
            Log.Debug("Melee weapon attack");
            if (currentComboAttackData == null)
            {
                currentComboAttackData = WeaponDataSet.GetCombo(0);
            }
            else
            {
                currentComboAttackData = currentComboAttackData.nextCombo;
            }
            if(_comboAttackQueue.Count > 0 && _comboAttackQueue.Last() == null) return;
            _comboAttackQueue.Enqueue(currentComboAttackData);
            if(currentComboAttackData.nextCombo == null)
            {
                currentComboAttackData = null;
                _comboAttackQueue.Enqueue(null);
            }
        }

        public override void Free()
        {
            
        }

        private void LateUpdate()
        {
            if(_comboAttackQueue.Count == 0) return;
            if(_isPlayingComboAttack) return;
            ExecuteComboAttack();
        }

        private async void ExecuteComboAttack()
        {
            _isPlayingComboAttack = true;

            while (_comboAttackQueue.Count > 0)
            {
                var comboAttackData = _comboAttackQueue.Peek();
                Log.Debug($"Skill name:{ comboAttackData?.comboName} - Level: {comboAttackData?.comboLevel}");
                float delay =  await _actor.AnimationController.AttackByAnimation(comboAttackData , this);
                await UniTask.Delay(TimeSpan.FromSeconds(delay/1.2f));
                _comboAttackQueue.Dequeue();
            }
            await UniTask.Delay(TimeSpan.FromSeconds(WeaponDataSet.attackRate));
            await _actor.AnimationController.AttackByAnimation(null , this);
            currentComboAttackData = null;
            _isPlayingComboAttack = false;
        }
    }
}