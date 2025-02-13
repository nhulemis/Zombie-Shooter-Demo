using _1_Game.Scripts.Systems;
using _1_Game.Scripts.Systems.AI.PathFinding;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using Game.UI;

namespace _1_Game.Scripts.GamePlay.Commands
{
    public class PrepareGameplaySystemCommand : ICommand
    {
        public async UniTask Execute()
        {
            new OpenGamePlayScreenCommand().Execute().Forget();
            Locator<InventorySystem>.Set(new InventorySystem());
            Locator<NavMeshProvider>.Set(new NavMeshProvider());
            await UniTask.CompletedTask;
        }
    }
}