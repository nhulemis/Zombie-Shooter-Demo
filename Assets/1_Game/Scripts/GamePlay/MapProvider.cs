using System.Collections.Generic;
using System.Linq;
using _1_Game.Scripts.Systems.Door;
using Cysharp.Threading.Tasks;
using Game.UI;
using NUnit.Framework;
using UnityEngine;

namespace _1_Game.Scripts.GamePlay
{
    public class MapProvider
    {
        public List<InteractiveDoorComponent> Doors = new ();
        public List<GameObject> Enemies = new ();
        
        public void AddEnemy(GameObject enemy)
        {
            Enemies.Add(enemy);
        }
        
        public void AddDoor(InteractiveDoorComponent door)
        {
            Doors.Add(door);
        }
        
        public async void  CheckPlayerHasCompleted()
        {
            await UniTask.NextFrame();
            bool allDoorsOpen = Doors.All(door => door.IsOpen);
            if (allDoorsOpen)
            {
                Debug.Log("Player has open all doors");
            }
            
            bool allEnemiesDead = Enemies.All(enemy => enemy == null);
            if (allEnemiesDead)
            {
                Debug.Log("Player has killed all enemies");
            }
            
            if(allDoorsOpen && allEnemiesDead)
            {
                Debug.Log("Player has completed the level");
                new OpenClearStagePopupCommand().Execute().Forget();
            }
        }
    }
}