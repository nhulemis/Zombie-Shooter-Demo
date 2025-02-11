using _1_Game.Scripts.Systems.Interactive;
using _1_Game.Scripts.Util;
using UnityEngine;

namespace _1_Game.Scripts.Systems
{
    public class ItemActorComponent : InteractiveComponent
    {
        [SerializeReference] private IInventoryItem _item;
        public override void React()
        {
            Locator<InventorySystem>.Get().AddItem(_item);
            Destroy(gameObject);
        }
    }
}