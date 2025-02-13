using _1_Game.Scripts.Systems.Pickup;
using _1_Game.Scripts.Util;
using _1_Game.Systems.Character;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace _1_Game.Scripts.Systems.WeaponSystem.Commands
{
    public class PickupWeaponCommand : ICommand
    {
        private IPickupableObject _pickupable;
        
        public PickupWeaponCommand(IPickupableObject weapon)
        {
            _pickupable = weapon;
        }
        
        public async UniTask Execute()
        {
            PlayerActor player = GameObject.FindAnyObjectByType<PlayerActor>();
            if (player == null) return;
            var weapon = _pickupable as Weapon;
            Assert.IsNotNull(weapon, "Class Weapon is not implement IPickupableObject");
            player.PickupWeapon(weapon);
        }
    }
}