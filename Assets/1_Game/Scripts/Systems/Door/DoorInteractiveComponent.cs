using System;
using System.Collections;
using _1_Game.Scripts.Systems.Interactive;
using _1_Game.Scripts.Util;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

namespace _1_Game.Scripts.Systems.Door
{
    public class InteractiveDoorComponent : DoorComponent
    {
        [SerializeField] private bool _interactable;
        [SerializeField, ReadOnly]
        private Canvas _canvas;
        [SerializeField , ValueDropdown("tmpTexts")] private TMP_Text _txtKeys;
        [SerializeField] private int requiredKeys = 1;
        private IEnumerable tmpTexts => transform.GetComponentsInChildren<TMP_Text>();
        

        private InventorySystem inventorySystem => Locator<InventorySystem>.Get();

        private void Start()
        {
            _canvas.gameObject.SetActive(false);
            OnPropertyChanged(typeof(KeyItem), 0);
            RegisterListeners();
        }

        private void RegisterListeners()
        {
            inventorySystem.Inventory.ObserveAdd().Subscribe(itemChanged =>
            {
                OnPropertyChanged(itemChanged.Key, itemChanged.Value);
            }).AddTo(this);
            inventorySystem.Inventory.ObserveReplace().Subscribe(itemChanged =>
            {
                OnPropertyChanged(itemChanged.Key, itemChanged.NewValue);
            }).AddTo(this);
        }
        
        private void OnPropertyChanged(Type itemChangedKey, int itemChangedValue)
        {
            if(itemChangedKey == typeof(KeyItem))
            {
                _txtKeys.text = $"{itemChangedValue}/{requiredKeys}";
            }
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