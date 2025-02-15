using UnityEngine;

namespace _1_Game.Scripts.GamePlay.RoomSpawner
{
    public interface ISpawnActor
    {
        GameObject Prefab { get; set; }
        void Spawn(Vector3 position);
        void Spawn(Transform parent);
    }
}