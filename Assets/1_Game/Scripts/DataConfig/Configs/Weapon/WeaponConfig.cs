using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Script.GameData.Weapon
{
    public class WeaponConfig : BaseConfig
    {
        [ListDrawerSettings(CustomAddFunction = "AddNewConfig", CustomRemoveElementFunction = "RemoveDataTable")]
        public List<WeaponDataSet> weaponDataSets = new List<WeaponDataSet>();
        protected override IList Items => weaponDataSets;
    }
}