using System;
using System.Collections;
using _1_Game.Scripts.DataConfig;
using _1_Game.Scripts.Systems.AddressableSystem;
using _1_Game.Scripts.Systems.WeaponSystem;
using _1_Game.Scripts.Systems.WeaponSystem.DamageEffect;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using Script.GameData;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _1_Game.Systems.Character
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(CharacterAnimationController))]
    [RequireComponent(typeof(WeaponController))]
    public class CharacterActor : MonoBehaviour
    {
        [SerializeField] protected CharacterAnimationController _animationController;
        [SerializeField] protected CharacterController _controller;
        [SerializeField] protected WeaponController _weaponController;
        [SerializeField, ValueDropdown("CharacterConfigID")] string _characterConfigID;
        
        public IEnumerable CharacterConfigID => IDGetter.GetCharacterConfigs();
        public CharacterAnimationController AnimationController => _animationController;
        
        public WeaponActorComponent Weapon => _weaponController.EquippedWeapon;
        
        public CharacterDataConfig CharacterDataConfig;
        protected float VerticalVelocity;
        protected bool isAiming;
        protected float health;
        public ReactiveProperty<bool> RxIsStunned { get; } = new();
        public bool IsStunned
        {
            get => RxIsStunned.Value;
            set => RxIsStunned.Value = value;
        }

        public WeaponController WeaponController => _weaponController;

        private void Awake()
        {
            CharacterDataConfig = SafetyDatabase.SafetyDB.Get<CharacterConfig>().Get(_characterConfigID);
            _weaponController.Init(this);
            _animationController.Init(CharacterDataConfig);
            health = CharacterDataConfig.Health;
            
        }

        public virtual void Attack()
        {
        }
        
        public virtual void Attack(Vector3 target)
        {
            _weaponController.AttackTo(target);
        }

        protected float VerticalMovement()
        {
            if (_controller.isGrounded)
            {
                VerticalVelocity = -1;
            }
            else
            {
                VerticalVelocity -= Physics.gravity.magnitude * (Time.deltaTime * CharacterDataConfig.Mass);
            }

            return VerticalVelocity;
        }

        private void OnValidate()
        {
            if (_animationController == null)
            {
                _animationController = GetComponent<CharacterAnimationController>();
            }
            
            if(_controller == null)
            {
                _controller = GetComponent<CharacterController>();
            }
            
            if(_weaponController == null)
            {
                _weaponController = GetComponent<WeaponController>();
            }
        }
        
        protected void OverrideCharacterConfig(string characterConfigID)
        {
            var overrideData = SafetyDatabase.SafetyDB.Get<CharacterConfig>().Get(characterConfigID);
            if(overrideData == null) return;
            CharacterDataConfig = overrideData;
            _animationController.ApplyOverrideConfig(CharacterDataConfig);
            Log.Debug("[Character] Character Config Overridden: " + characterConfigID);
        }
        
        public async void ExecuteGrenade(WeaponActorComponent grenade)
        {
            PickupWeapon(grenade);
            var grenadeThrow = _weaponController.EquippedWeapon;
            float timeToPlayAnim = _animationController.Execute_GrenadeThrow(grenadeThrow);
            await UniTask.Delay(TimeSpan.FromSeconds(timeToPlayAnim / 6));
            _weaponController.ThrowGrenade();
            RetrieveWeaponFromIdleHand();
        }
        
        private void RetrieveWeaponFromIdleHand()
        {
            _weaponController.RetrieveWeaponFromIdleHand();
            _animationController.EquipWeapon(_weaponController.EquippedWeapon);
            OverrideCharacterConfig(_weaponController.EquippedWeapon.WeaponDataSet.overrideCharacterDataConfig);
        }
        
        public void Execute_MovementAnimation(CharacterAnimationController.MovementParameters parameters)
        {
            _animationController.Execute_MovementAnimation(parameters);
        }
        
        public void TakeDamage(float damage)
        {
            Log.Debug($"[Character] {name} took {damage} damage");
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }

        private async void Die()
        {
            _controller.enabled = false;
            await _animationController.Execute_DeathAnimation();
            this.enabled = false;
            Destroy(gameObject);
        }

        public void PickupWeapon(WeaponActorComponent weapon , bool switchToIdleHand = true)
        {
            if (switchToIdleHand)
            {
                _weaponController.SwitchWeaponToIdleHand();
            }
            else
            {
                _weaponController.Drop();
            }
            _weaponController.EquipWeapon(weapon);
            _animationController.EquipWeapon(weapon);
            OverrideCharacterConfig(weapon.WeaponDataSet.overrideCharacterDataConfig);
        }

        public async void ApplyDamageEffect(IDameEffectAction damageEffect)
        {
            damageEffect.ApplyEffect(this);
            var fxPrefab = await AssetLoader.Load<GameObject>(damageEffect.EffectPrefab);
            var fx = Instantiate(fxPrefab, transform.position, Quaternion.identity);
            Destroy(fx, damageEffect.EffectDuration);
            
        }
        
        public async void TakeStun(float duration)
        {
            Log.Debug($"[Character] {name} is stunned for {duration} seconds");
            IsStunned = true;
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            IsStunned = false;
        }

        public string PickSpell()
        {
            // random spell
            if(CharacterDataConfig.SpellIds.Count  > 0)
            {
                return CharacterDataConfig.SpellIds[UnityEngine.Random.Range(0, CharacterDataConfig.SpellIds.Count)];
            }
            return String.Empty;
        }

        public async void CastSpell(SpellDataSet spell, Transform target)
        {
            Log.Debug($"[Character] CastSpell {spell.id}");
            bool result = await spell.beforeCastSpellActorCommand.Make(this).Execute();
            if(result == false) return;
            var prefab = await AssetLoader.Load<GameObject>(spell.spellRefKey);
            var spellInstance = Instantiate(prefab, transform.position, Quaternion.identity);
            var castSpellComponent = spellInstance.GetComponent<CastSpellPositionActorComponent>();
            castSpellComponent.Init(spell, target, () =>
            {
                Attack(target.position);
            });
            castSpellComponent.CastSpell();
            await UniTask.WaitUntil(castSpellComponent.IsSpellFinished);
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            RetrieveWeaponFromIdleHand();
        }
    }
}