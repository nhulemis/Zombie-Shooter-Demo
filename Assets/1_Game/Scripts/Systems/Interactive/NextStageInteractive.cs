using System.Collections.Generic;
using _1_Game.Scripts.Systems.Observe;
using _1_Game.Scripts.Util;
using UnityEngine;

namespace _1_Game.Scripts.Systems.Interactive
{
    public class NextStageInteractiveComponent : InteractiveComponent
    {
        public List<GameObject> ActivateObjects;
        public List<GameObject> DeactivateObjects;
        [SerializeReference] private List<ICommand> _commands = new List<ICommand>();

        private bool _isActivated = false;
        public override void React()
        {
            if (_isActivated) return;
            _isActivated = true;
            Log.Debug("Load NextStage");
            base.React();
            foreach (var obj in ActivateObjects)
            {
                obj.SetActive(true);
            }
            foreach (var obj in DeactivateObjects)
            {
                obj.SetActive(false);
            }
            
            foreach (var command in _commands)
            {
                command.Execute();
            }
            
            Locator<DoorObserver>.Get().NextStage();
        }
    }
}