using Cysharp.Threading.Tasks;

namespace _1_Game.Scripts.Util
{
    public interface ICommand
    {
        UniTask Execute();
    }
}