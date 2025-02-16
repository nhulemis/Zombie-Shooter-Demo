using System;
using _1_Game.Scripts.Systems.AddressableSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _1_Game.Scripts.GamePlay.RoomSpawner
{
    [Serializable]
    public class SpawnEnemyActor : ISpawnActor
    {
        [field: SerializeField] public GameObject Prefab { get; set; }
        
        public void Spawn(Vector3 position)
        {
           _= AssetLoader.Instantiate(Prefab, position, Quaternion.identity);
        }

        public async void Spawn(Transform parent)
        {
            var go = UnityEngine.Object.Instantiate(Prefab, parent);
            go.transform.localPosition = Vector3.zero;
            await UniTask.NextFrame();
            go.transform.SetParent(null);
        }
    }
}