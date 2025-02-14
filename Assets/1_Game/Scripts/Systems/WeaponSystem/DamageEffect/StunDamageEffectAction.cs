using _1_Game.Systems.Character;
using UnityEngine;

namespace _1_Game.Scripts.Systems.WeaponSystem.DamageEffect
{
    public class StunDamageEffectAction : BaseDamageEffectAction
    {
        public override void ApplyEffect(CharacterActor effectPosition)
        {
            Log.Debug("Stun effect");
            effectPosition.TakeStun(EffectDuration);
        }
    }
}