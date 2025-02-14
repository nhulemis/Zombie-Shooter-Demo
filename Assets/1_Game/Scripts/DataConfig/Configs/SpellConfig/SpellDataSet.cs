using _1_Game.Scripts.Systems.AddressableSystem;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using Script.GameData;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace Script.GameData
{
    public class SpellDataSet : BaseRecord
    {
        public AssetReference spellRefKey;
        public float castTime = 1f;
        public float cooldown = 10f;
        [Header("Command")]
        [SerializeReference]
        public IActorCommand beforeCastSpellActorCommand;
        [SerializeReference]
        public IActorCommand afterCastSpellActorCommand;
    }
}