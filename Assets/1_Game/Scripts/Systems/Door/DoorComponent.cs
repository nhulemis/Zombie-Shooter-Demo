using System.Collections;
using System.Collections.Generic;
using _1_Game.Scripts.Systems.AI.PathFinding;
using _1_Game.Scripts.Systems.Interactive;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _1_Game.Scripts.Systems.Door
{
    public class DoorComponent : InteractiveComponent
    {
        [SerializeField,ValueDropdown("GetDoorParts")] Transform[] _doorParts;
        [SerializeReference] private List<ICommand> _doorStateChangeCommands = new List<ICommand>();
        public bool IsOpen { get; set; }
        private IEnumerable GetDoorParts()
        {
            var doorParts = GetComponentsInChildren<Transform>();   
            return doorParts;
        }
        
        public override void Execute()
        {
            OpenDoor();
        }
        
        protected async void OpenDoor()
        {
            List<UniTask> tasks = new List<UniTask>();
            foreach (var doorPart in _doorParts)
            {
                tasks.Add(doorPart.DOLocalMoveY(-5f, 1f).SetEase(Ease.InOutSine).ToUniTask());
            }
            await UniTask.WhenAll(tasks);
            ChangeDoorState();
        }
        
        protected async void CloseDoor()
        {
            List<UniTask> tasks = new List<UniTask>();
            foreach (var doorPart in _doorParts)
            {
                tasks.Add(doorPart.DOLocalMoveY(0f, 1f).SetEase(Ease.InOutSine).ToUniTask());
            }
            await UniTask.WhenAll(tasks);
            ChangeDoorState();
        }
        
        protected void ChangeDoorState()
        {
            foreach (var command in _doorStateChangeCommands)
            {
                command.Execute();
            }
        }
    }
}