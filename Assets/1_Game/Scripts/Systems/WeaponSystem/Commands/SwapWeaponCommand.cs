using System.Collections;
using _1_Game.Scripts.Util;
using _1_Game.Systems.Character;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

namespace _1_Game.Scripts.Systems.WeaponSystem.Commands
{
    public class SwapWeaponCommand : IActorCommand
    {
        [ValueDropdown("Internal_GetTypes")]
        public string SwapToWeaponActorComponent;
        private CharacterActor _owner;
        
        private IEnumerable Internal_GetTypes()
        {
            var types = TypeHelper.GetTypesInFolder<WeaponActorComponent>("_1_Game.Scripts");
            var result = new ValueDropdownList<string>();
            foreach (var type in types)
            {
                result.Add(type.Name, type.ToString());
            }
            
            types = TypeHelper.GetTypesInFolder<IRangeActor>("_1_Game.Scripts");
            foreach (var type in types)
            {
                result.Add(type.Name, type.ToString());
            }
            return result;
        }

        public SwapWeaponCommand()
        {
            
        }

        public SwapWeaponCommand(string swapToWeaponActorComponent)
        {
            SwapToWeaponActorComponent = swapToWeaponActorComponent;
        }
        
        public IActorCommand Make(CharacterActor actor)
        {
            _owner = actor;
            return this;
        }

        public async UniTask<bool> Execute()
        {
            Assert.IsFalse(string.IsNullOrEmpty(SwapToWeaponActorComponent), "SwapToWeaponActorComponent is null or empty");
            Assert.IsNotNull(_owner, "Owner WeaponController is null, please call Make() method first");
            bool isEquipped = false;
            
            foreach (var weapon in _owner.WeaponController.Weapons)
            {
                if (weapon.GetType() == typeof(RangeWeapon))
                {
                    var rangeWeapon = weapon as RangeWeapon;
                    if(rangeWeapon != null && rangeWeapon.GetRangeActorType.ToString() == SwapToWeaponActorComponent)
                    {
                        _owner.PickupWeapon(weapon);
                        isEquipped = true;
                        break;
                    }
                }
                if (weapon.GetType().ToString() == SwapToWeaponActorComponent)
                {
                    _owner.PickupWeapon(weapon);
                    isEquipped = true;
                    break;
                }
            }

            if (!isEquipped)
            {
                Log.Warning($"Cannot find weapon {SwapToWeaponActorComponent} at the {_owner.name}.WeaponController.Weapon[] to swap");
            }
            return isEquipped;
        }
    }
}