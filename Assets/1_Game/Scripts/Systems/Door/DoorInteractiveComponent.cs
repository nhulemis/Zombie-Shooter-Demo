using System;
using _1_Game.Scripts.Systems.Interactive;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace _1_Game.Scripts.Systems.Door
{
    public class InteractiveDoorComponent : DoorComponent
    {
        [SerializeField] private bool _interactable;
        [SerializeField, ReadOnly]
        private Canvas _canvas;

        private void Start()
        {
            _canvas.gameObject.SetActive(false);
        }

        private void OnValidate()
        {
            if (_canvas == null)
            {
                _canvas = new GameObject("WidgetInteraction").AddComponent<Canvas>();
                _canvas.transform.SetParent(transform);
                _canvas.transform.localPosition = Vector3.up * 2;
                _canvas.renderMode = RenderMode.WorldSpace;
            }
        }

        public override void React()
        {
            if(!_interactable) return;
            
            _canvas.gameObject.SetActive(true);
        }
        
        public override void ReactEnd()
        {
            _canvas.gameObject.SetActive(false);
        }
        
        public override void Execute()
        {
           base.Execute();
           _canvas.gameObject.SetActive(false);
        }

        private void LateUpdate()
        {
            if (Camera.main != null)
            {
                // canvas look at the opposite direction of the camera
                Vector3 directionAway = _canvas.transform.position - Camera.main.transform.position;

                // Make canvas look in the opposite direction
                _canvas.transform.LookAt(_canvas.transform.position + directionAway);
            }
        }
    }
}