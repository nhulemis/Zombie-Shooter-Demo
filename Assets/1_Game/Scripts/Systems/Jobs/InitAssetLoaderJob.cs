using _1_Game.Scripts.Systems.AddressableSystem;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;

namespace _1_Game.Scripts.Systems
{
    public class InitAssetLoaderJob : ICommand
    {
        public UniTask Execute()
        {
            Locator<AssetLoader>.Set(new AssetLoader());
            return UniTask.CompletedTask;
        }
    }
}