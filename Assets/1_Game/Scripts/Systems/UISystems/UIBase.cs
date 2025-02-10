using System.Collections;
using _1_Game.Scripts.DataConfig;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using Script.GameData.UIConfigs;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Systems.UI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class UIBase : MonoBehaviour
    {
        private bool _isClosed;
        [SerializeField, ValueDropdown("ViewGroups"), FoldoutGroup("Base Setting")] private string _uiGroupName;
        [SerializeField, FoldoutGroup("Base Setting")] private UIBuilder _builderName;

#if UNITY_EDITOR
        public IEnumerable ViewGroups => IDGetter.GetUIGroupName();        
#endif
        // Called when the UI is shown
        public virtual async UniTask OnShow(params object[] args)
        {
            _isClosed = false;
            Debug.Log($"UI {name} OnShow called.");
            await UniTask.Yield(); // Simulate asynchronous setup if necessary
        }

        // Called to wait until the UI is closed
        public async UniTask OnClose()
        {
            Debug.Log($"Waiting for UI {name} to close...");
            // Wait until the UI is marked as closed
            await UniTask.WaitUntil(() => _isClosed);
            Debug.Log($"UI {name} OnClose completed.");
        }

        // Call this to close the UI
        public void CloseUI()
        {
            Debug.Log($"UI {name} is closing.");
            _isClosed = true;
        }

#if UNITY_EDITOR
        private GameDataBase gameDataBase => SafetyDatabase.SafetyDB;
        
        [Button]
        public void RegisterUI()
        {
            var uiConfig = gameDataBase.Get<UIConfig>();
            EditorUtility.SetDirty(uiConfig);
            var uiData = uiConfig.Get(name);
            if (uiData == null)
            {
                uiData = new UIDataConfig
                {
                    id = name,
                    viewGroup = _uiGroupName,
                    prefabPath = $"UI/{name}/{name}",
                    builder = _builderName
                };
                uiConfig.UIDataConfigs.Add(uiData);
            }
            else
            {
                uiData.viewGroup = _uiGroupName;
                uiData.builder = _builderName;
                uiData.prefabPath = $"UI/{name}/{name}";
            }
            Debug.Log($"UI {name} registered.");
            EditorUtility.SetDirty(uiConfig);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
#endif
        
    }
}