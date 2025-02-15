using UniRx;

namespace _1_Game.Scripts.Systems.Observe
{
    public class DoorObserver
    {
        public ReactiveCommand OnUserOpenDoor { get; } = new ();
        public ReactiveProperty<bool> RxIsDoorOpen { get; } = new();
        
        public void OpenDoor()
        {
            OnUserOpenDoor.Execute();
            RxIsDoorOpen.Value = true;
        }
    }
}