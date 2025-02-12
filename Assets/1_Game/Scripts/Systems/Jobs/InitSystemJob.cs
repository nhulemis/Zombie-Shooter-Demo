using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;

namespace _1_Game.Scripts.Systems
{
    public class InitSystemJob : ICommand
    {
        public async UniTask Execute()
        {
            await new InitDatabaseJob().Execute();
            await new InitUISystem().Execute();
            //await new InitAdSystemJob().Execute();
            await new InitAssetLoaderJob().Execute();
            await UniTask.CompletedTask;
        }
    }
}