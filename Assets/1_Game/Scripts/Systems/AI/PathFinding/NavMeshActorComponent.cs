using System;
using _1_Game.Scripts.Util;
using Unity.AI.Navigation;
using UnityEngine;

namespace _1_Game.Scripts.Systems.AI.PathFinding
{
    [RequireComponent(typeof(NavMeshSurface))]
    public class NavMeshActorComponent : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface _navMeshSurface;

        private void OnValidate()
        {
            if (_navMeshSurface == null)
            {
                _navMeshSurface = GetComponent<NavMeshSurface>();
            }
        }

        private void Start()
        {
            Locator<NavMeshProvider>.Get().RegisterNavMeshSurface(BuildNavMesh);
        }

        private void OnDestroy()
        {
            Locator<NavMeshProvider>.Get().UnregisterNavMeshSurface(BuildNavMesh);
        }

        private void BuildNavMesh()
        {
            _navMeshSurface.BuildNavMesh();
        }
    }
}