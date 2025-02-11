using _1_Game.Scripts.Systems;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;

namespace _1_Game.Scripts.GamePlay.Commands
{
    public class PrepareOutGameplayCommand : ICommand
    {
        public async UniTask Execute()
        {
            Locator<InventorySystem>.Release();
            await UniTask.CompletedTask;
        }
    }
}