using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _1_Game.Scripts.GamePlay.RoomSpawner
{
    [Serializable]
    public class SpawnItem : ISpawnActor
    {
        [field: SerializeField]
        public GameObject Prefab { get; set; }
        public void Spawn(Vector3 position)
        {
            Object.Instantiate(Prefab, position, Quaternion.identity);
        }
    }
}