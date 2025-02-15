using _1_Game.Scripts.Systems.Observe;
using _1_Game.Scripts.Util;
using UniRx;
using UnityEngine;

namespace _1_Game.Scripts.Systems.Door
{
    public class AutoDoorComponent : DoorComponent
    {
        [SerializeField] bool openOnStart = false;
        private void Start()
        {
            Locator<DoorObserver>.Get().OnUserOpenDoor.Subscribe(_ =>
            {
                OpenDoor();
            }).AddTo(this);
            if (openOnStart)
            {
                OpenDoor();
            }
        }

        public override void ReactEnd()
        {
            if (openOnStart)
            {
                return;
            }
            CloseDoor();
        }
        
        public override void React()
        {
            OpenDoor();
        }
    }
}