using _1_Game.Scripts.DataConfig;
using _1_Game.Scripts.Systems.AddressableSystem;
using _1_Game.Scripts.Util;
using _1_Game.Systems.Character;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using Script.GameData.AddressableMapping;
using UnityEngine;

namespace _1_Game.Scripts.Systems.WeaponSystem.Commands
{
    public class PreStunGrenadeCommand : ICommand
    {
        private GameDataBase gameDataBase => Locator<GameDataBase>.Get();
        public async UniTask Execute()
        {
            var player = GameObject.FindAnyObjectByType<PlayerActor>();
            Assert.IsNotNull(player, "Player not found");
            var key = gameDataBase.Get<AddressableMappingConfig>().GetPathId<StunGrenadeActor>();
            Assert.IsNotNull(key, "the key type GrenadeActor not defined in AddressableMappingConfig");
            var grenadeGameObject = await AssetLoader.Load<GameObject>(key);
            var grenade = grenadeGameObject.GetComponent<Weapon>();
            Assert.IsNotNull(grenade, "Weapon not found in the prefab");
            player.ExecuteGrenade(grenade);
        }
    }
}