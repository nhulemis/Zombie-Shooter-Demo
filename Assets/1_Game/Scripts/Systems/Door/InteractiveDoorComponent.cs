using System;
using System.Collections;
using _1_Game.Scripts.GamePlay;
using _1_Game.Scripts.Systems.Interactive;
using _1_Game.Scripts.Systems.Observe;
using _1_Game.Scripts.Util;
using Game.Systems.UI;
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
        [SerializeField] private float _unlockTime = 5f;
        
        [SerializeField, ValueDropdown("ViewGroups"), FoldoutGroup("Interactive")] private string _uiGroupName;
        [SerializeField, FoldoutGroup("Interactive")] private UIBuilder _builderName;
        [SerializeField, FoldoutGroup("Interactive")] private OpenDoorActorComponent _doorActor;
#if UNITY_EDITOR
        public IEnumerable ViewGroups => IDGetter.GetUIGroupName();        
#endif
        
        private IEnumerable tmpTexts => transform.GetComponentsInChildren<TMP_Text>();
        private bool _isUnlocking;
        private float _unlockTimer;

        private InventorySystem inventorySystem => Locator<InventorySystem>.Get();
        
        private Transform _interactiveView;

        private void Start()
        {
            _canvas.gameObject.SetActive(false);
            _interactiveView = _doorActor.transform;
            OnPropertyChanged(typeof(KeyItem), 0);
            RegisterListeners();
            var view = Locator<UISystem>.Get().GetView(_uiGroupName);
            var builder = view.GetBuilder(_builderName);
            _interactiveView.SetParent(builder);
            _interactiveView.gameObject.SetActive(false);
            _interactiveView.localRotation = Quaternion.Euler(0, 0, 0);
            if (_interactable)
            {
                Locator<MapProvider>.Get().AddDoor(this);
            }
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
            if(!_interactable || IsOpen) return;
            
            _canvas.gameObject.SetActive(true);
            _interactiveView.gameObject.SetActive(true);
        }
        
        public override void ReactEnd()
        {
            _canvas.gameObject.SetActive(false);
            _interactiveView.gameObject.SetActive(false);
            _isUnlocking = false;
        }
        
        public override void Execute()
        {
           base.Execute();
           _canvas.gameObject.SetActive(false);
           _interactiveView.gameObject.SetActive(false);
           _unlockTimer = 0;
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
            
            if (_isUnlocking)
            {
                _unlockTimer += Time.deltaTime;
                _doorActor.OnOpenDoor(_unlockTimer / _unlockTime);
                if (_unlockTimer >= _unlockTime)
                {
                    _isUnlocking = false;
                    OpenDoor();
                    inventorySystem.Use<KeyItem>(requiredKeys);
                    IsOpen = true;
                    Locator<MapProvider>.Get().CheckPlayerHasCompleted();
                }
            }
            
            StickyInterActiveView();
        }

        private void StickyInterActiveView()
        {
            if (Camera.main != null)
            {
                _interactiveView.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2);
            }
        }

        public void UnlockDoor()
        {
            if (inventorySystem.Inventory.ContainsKey(typeof(KeyItem)) && inventorySystem.Inventory[typeof(KeyItem)] >= requiredKeys)
            {
                _isUnlocking = true;
                Locator<DoorObserver>.Get().OpenDoor();
            }
        }
    }
}