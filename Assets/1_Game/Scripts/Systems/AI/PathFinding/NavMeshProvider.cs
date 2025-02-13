using System;
using Unity.AI.Navigation;

namespace _1_Game.Scripts.Systems.AI.PathFinding
{
    public class NavMeshProvider 
    {
        private Action _navMeshBuildAction;

        public void RegisterNavMeshSurface(Action navMeshBuildAction)
        {
            _navMeshBuildAction += navMeshBuildAction;
        }
        
        public void UnregisterNavMeshSurface(Action navMeshBuildAction)
        {
            _navMeshBuildAction -= navMeshBuildAction;
        }
        
        public void BuildNavMesh()
        {
            _navMeshBuildAction?.Invoke();
        }
    }
}