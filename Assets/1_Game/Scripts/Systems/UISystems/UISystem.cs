using System;
using System.Collections.Generic;
using _1_Game.Scripts.DataConfig;
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using Script.GameData.UIConfigs;
using UnityEngine;

namespace Game.Systems.UI
{
    public class UISystem
    {
        // Singleton Instance
        //public static UISystem Instance => Locator<UISystem>.Instance;

        // Store all registered views
        private readonly Dictionary<string, UIView> _views = new Dictionary<string, UIView>();

        // Store active UI instances
        private readonly Dictionary<string, UIBase> _activeUIs = new Dictionary<string, UIBase>();

        private GameDataBase GameDataBase => Locator<GameDataBase>.Instance;
        private UIConfig UIConfig => GameDataBase.Get<UIConfig>();
        
        public static async UniTask AsyncShow<T>(params object[] args) where T : UIBase
        {
            await Locator<UISystem>.Instance.Show<T>(args);
        }

        // Register a UIView
        public void RegisterView(UIView view)
        {
            if (_views.TryAdd(view.GroupId, view))
            {
                Debug.Log($"Registered UIView: {view.name}");
            }
        }

        public async UniTask Show<T>(params object[] args) where T : UIBase
        {
            var uiName = typeof(T).Name;
            var uiDataConfig = UIConfig.Get(uiName);
            if(uiDataConfig == null)
            {
                Debug.LogError($"UI Data Config not found for {uiName}");
                return;
            }
            await ShowUI(uiDataConfig, args);
        }

        // Show a UI and wait until it is closed
        private async UniTask ShowUI(UIDataConfig uiData, params object[] args)
        {
            if (!_views.TryGetValue(uiData.viewGroup, out var targetView))
            {
                Debug.LogError($"Builder {uiData.viewGroup} not found in registered views.");
                return;
            }

            // Load the prefab from Resources
            GameObject uiPrefab = Resources.Load<GameObject>(uiData.prefabPath);
            if (uiPrefab == null)
            {
                Debug.LogError($"UI Prefab not found at path: {uiData.prefabPath}");
                return;
            }

            // Instantiate the UI
            GameObject uiInstance = UnityEngine.Object.Instantiate(uiPrefab, targetView.GetBuilder(uiData.builder));
            UIBase uiBase = uiInstance.GetComponent<UIBase>();

            if (uiBase == null)
            {
                Debug.LogError($"UI Prefab {uiPrefab.name} does not contain a UIBase component.");
                UnityEngine.Object.Destroy(uiInstance);
                return;
            }

            // Track the active UI
            _activeUIs[uiData.id] = uiBase;
            
            //Unloard the uiPrefab
            Resources.UnloadUnusedAssets();

            // Call OnShow and wait for it to close
            await uiBase.OnShow(args);
            await uiBase.OnClose();
            // Cleanup after the UI is closed
            _activeUIs.Remove(uiData.id);
            UnityEngine.Object.Destroy(uiInstance);
        }
    }
}