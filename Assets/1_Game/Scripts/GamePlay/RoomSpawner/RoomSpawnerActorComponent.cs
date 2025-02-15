using System;
using System.Collections.Generic;
using _1_Game.Scripts.Systems.Observe;
using _1_Game.Scripts.Util;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _1_Game.Scripts.GamePlay.RoomSpawner
{
    public class RoomSpawnerActorComponent : MonoBehaviour
    {
        [SerializeField] private Transform _roomSpawnPoint;

        [SerializeField, MinMaxSlider(0, 20, true)]
        private Vector2Int _roomCount = new(5, 14);

        [SerializeField, FoldoutGroup("Spawner")]
        private List<SpawnConfig> _spawnConfigs;

        [SerializeField, ReadOnly, FoldoutGroup("Spawner")]
        private List<Transform> _spawnPoints;

        [SerializeField, FoldoutGroup("Spawner")]
        private int _initAmount = 5;

        private int pointIndex = 0;

        [Serializable]
        public class SpawnConfig
        {
            public int Amount;
            [SerializeReference] public ISpawnActor SpawnActor;
        }

        private void OnValidate()
        {
            if (_roomSpawnPoint == null)
            {
                _roomSpawnPoint = new GameObject("RoomSpawnPoint").transform;
                _roomSpawnPoint.SetParent(transform);
                _roomSpawnPoint.localPosition = Vector3.zero;
            }
        }

        private void Start()
        {
            RefreshSpawnPoint();
            for (int i = 0; i < _initAmount; i++)
            {
                Spawn();
            }
            Locator<DoorObserver>.Get().OnUserOpenDoor.Subscribe(_ =>
            {
                foreach (var config in _spawnConfigs)
                {
                    while (config.Amount > 0)
                    {
                        if (pointIndex >= _spawnPoints.Count) pointIndex = 0;
                        config.SpawnActor.Spawn(_spawnPoints[pointIndex]);
                        config.Amount--;
                        pointIndex++;
                    }
                }
            }).AddTo(this);
        }

        private void Spawn()
        {
            foreach (var spawnConfig in _spawnConfigs)
            {
                if (spawnConfig.Amount <= 0) continue;
                if (pointIndex >= _spawnPoints.Count) pointIndex = 0;
                spawnConfig.SpawnActor.Spawn(_spawnPoints[pointIndex]);
                spawnConfig.Amount--;
                pointIndex++;
                return;
            }
        }

        [Button]
        public void RefreshSpawnPoint()
        {
            while (_roomSpawnPoint.childCount > 0)
            {
                DestroyImmediate(_roomSpawnPoint.GetChild(0).gameObject);
            }

            _spawnPoints.Clear();
            var colliders = GetComponentsInChildren<BoxCollider>();
            if (colliders.Length == 0) return;
            colliders.Shuffle();
            int spawnPointCount = Random.Range(_roomCount.x, _roomCount.y);

            var prefab = Resources.Load<GameObject>("Prefabs/RoomSpawnPoint");
            if (prefab == null) return;
            foreach (var boxCollider in colliders)
            {
                if (spawnPointCount <= 0) break;
                if (!boxCollider.gameObject.name.Contains("Floor")) continue;

                var spawnPoint = Instantiate(prefab, _roomSpawnPoint);
                spawnPoint.transform.position = boxCollider.bounds.center + Vector3.up;
                spawnPointCount--;
                _spawnPoints.Add(spawnPoint.transform);
            }
        }
    }
}