namespace _1_Game.Scripts.Systems.Door
{
    public class AutoDoorComponent : DoorComponent
    {
        public override void ReactEnd()
        {
            CloseDoor();
        }
        
        public override void React()
        {
            OpenDoor();
        }
    }
}