using System;
using System.Collections.Generic;
using Sirenix.Serialization;
using UniRx;

namespace _1_Game.Scripts.Systems
{
    public class InventorySystem
    {
        public ReactiveDictionary<Type , int> Inventory = new ();
        
        public void AddItem(IInventoryItem item)
        {
            var type = item.GetType();
            if (!Inventory.TryAdd(type, 1))
            {
                Inventory[type]++;
            }
        }
    }
}