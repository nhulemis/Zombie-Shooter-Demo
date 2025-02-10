using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using Game.Systems.UI;

namespace _1_Game.Scripts.Systems
{
    public class InitUISystem : ICommand
    {
        public UniTask Execute()
        {
            Locator<UISystem>.Set(new UISystem());
            return UniTask.CompletedTask;
        }
    }
}