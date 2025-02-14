using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

namespace Script.GameData
{
    public class SpellConfig : BaseConfig
    {
        [ListDrawerSettings(CustomAddFunction = "AddNewConfig", CustomRemoveElementFunction = "RemoveDataTable"), InlineEditor]
        public List<SpellDataSet> spellDataSets;

        protected override IList Items => spellDataSets;
        
        public IEnumerable GetSpellDataSets()
        {
            return spellDataSets.Select(x=>x.id);
        }
        
        public SpellDataSet GetSpellDataSet(string id)
        {
            return spellDataSets.FirstOrDefault(x => x.id == id);
        }
    }
}