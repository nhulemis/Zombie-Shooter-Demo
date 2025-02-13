using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;

namespace _1_Game.Scripts.Systems.AI.PathFinding
{
    public class ReBuildNavMeshCommand : ICommand
    {
        public UniTask Execute()
        {
            Locator<NavMeshProvider>.Get()?.BuildNavMesh();
            return UniTask.CompletedTask;
        }
    }
}