using _1_Game.Systems.Character;
using Cysharp.Threading.Tasks;

namespace _1_Game.Scripts.Util
{
    public interface ICommand
    {
        UniTask Execute();
    }
    
    public interface IActorCommand
    {
        IActorCommand Make(CharacterActor actor);
        UniTask<bool> Execute();
    }
    
    public interface ICommandUI
    {
        UniTask Execute();
    }
}