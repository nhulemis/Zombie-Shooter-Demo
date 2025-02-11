using System.Collections;
using _1_Game.Scripts.Systems.Interactive;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _1_Game.Scripts.Systems.Door
{
    public class DoorComponent : InteractiveComponent
    {
        [SerializeField,ValueDropdown("GetDoorParts")] Transform[] _doorParts;
        
        private IEnumerable GetDoorParts()
        {
            var doorParts = GetComponentsInChildren<Transform>();   
            return doorParts;
        }
        
        public override void Execute()
        {
            OpenDoor();
        }
        
        protected void OpenDoor()
        {
            foreach (var doorPart in _doorParts)
            {
                doorPart.DOLocalMoveY(-5f, 1f).SetEase(Ease.InOutSine);
            }
        }
        
        protected void CloseDoor()
        {
            foreach (var doorPart in _doorParts)
            {
                doorPart.DOLocalMoveY(0f, 1f).SetEase(Ease.InOutSine);
            }
        }
    }
}