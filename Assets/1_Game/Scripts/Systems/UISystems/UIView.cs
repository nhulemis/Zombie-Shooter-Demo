using System.Collections;
using System.Collections.Generic;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Systems.UI
{
    public class UIView : MonoBehaviour
    {
        [ValueDropdown("ViewGroups")]
        public string GroupId; 
        // Optional, for easier identification
        public RectTransform MainViews;
        public RectTransform DialogViews;
        public RectTransform OverlayViews;

        // A dictionary for dynamic builder access
        private readonly Dictionary<UIBuilder, RectTransform> _builders = new Dictionary<UIBuilder, RectTransform>();
#if UNITY_EDITOR
        public IEnumerable ViewGroups => IDGetter.GetUIGroupName();        
#endif
        private void Awake()
        {
            // Initialize builder references
            _builders[UIBuilder.MainView] = MainViews;
            _builders[UIBuilder.DialogView] = DialogViews;
            _builders[UIBuilder.OverlayView] = OverlayViews;

            // Register this view to UISystem
            Locator<UISystem>.Instance.RegisterView(this);
        }

        public RectTransform GetBuilder(UIBuilder builderName)
        {
            if (_builders.TryGetValue(builderName, out var builder))
            {
                return builder;
            }

            Debug.LogError($"Builder {builderName} not found in UIView {name}.");
            return null;
        }

        private void Start()
        {
            DebugLoadGamePlayUI();
        }

        [Button]
        public async void DebugLoadGamePlayUI()
        {
        }
    }
}