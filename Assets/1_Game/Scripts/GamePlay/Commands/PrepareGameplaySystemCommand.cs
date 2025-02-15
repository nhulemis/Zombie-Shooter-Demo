using _1_Game.Scripts.Systems;
using _1_Game.Scripts.Systems.AI.PathFinding;
using _1_Game.Scripts.Systems.Observe;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using Game.UI;

namespace _1_Game.Scripts.GamePlay.Commands
{
    public class PrepareGameplaySystemCommand : ICommand
    {
        public async UniTask Execute()
        {
            Locator<InventorySystem>.Set(new InventorySystem());
            Locator<NavMeshProvider>.Set(new NavMeshProvider());
            Locator<DoorObserver>.Set(new DoorObserver());
            Locator<MapProvider>.Set(new MapProvider());
            await new OpenGamePlayScreenCommand().Execute();
            await UniTask.CompletedTask;
        }
    }
}