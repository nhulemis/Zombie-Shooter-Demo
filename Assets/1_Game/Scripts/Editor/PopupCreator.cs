using System;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class PopupCreator : EditorWindow
{
    private string popupName = "NewPopup";

    [MenuItem("Tools/UI System/Create Popup")]
    private static void ShowWindow()
    {
        // Show the editor window
        GetWindow<PopupCreator>("Popup Creator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Create New Popup", EditorStyles.boldLabel);

        // Input field for the popup name
        popupName = EditorGUILayout.TextField("Popup Name", popupName);

        if (GUILayout.Button("Create Popup"))
        {
            if (string.IsNullOrEmpty(popupName))
            {
                EditorUtility.DisplayDialog("Error", "Popup name cannot be empty!", "OK");
                return;
            }

            CreatePopup(popupName);
        }
    }

    private async void CreatePopup(string popupName)
    {
        // 1. Create the script
        string scriptPath = $"Assets/1_Game/Scripts/UI/{popupName}/{popupName}.cs";
        if (!Directory.Exists($"Assets/1_Game/Scripts/UI/{popupName}"))
        {
            Directory.CreateDirectory($"Assets/1_Game/Scripts/UI/{popupName}");
        }

        if (File.Exists(scriptPath))
        {
            EditorUtility.DisplayDialog("Error", "Script already exists!", "OK");
            return;
        }

        string scriptUIContent = $@"
using System;
using Cysharp.Threading.Tasks;
using Game.Systems.UI;
using UnityEngine;

namespace Game.UI
{{
    public class {popupName} : UIBase
    {{
        public override async UniTask OnShow(params object[] args)
        {{
            await base.OnShow(args);
            // Implement the Show method
        }}
    }}
}}
";
        await File.WriteAllTextAsync(scriptPath, scriptUIContent);
        AssetDatabase.Refresh(); // Refresh the AssetDatabase so the new script appears in the Project window

        string scriptProviderContent = $@"
using System;

namespace Game.UI
{{
    public class {popupName}Provider : IDisposable
    {{
        public void Dispose()
        {{
        }}
    }}
}}
";
        string scriptProviderPath = $"Assets/1_Game/Scripts/UI/{popupName}/{popupName}Provider.cs";
        await File.WriteAllTextAsync(scriptProviderPath, scriptProviderContent);
        AssetDatabase.Refresh(); // Refresh the AssetDatabase so the new script appears in the Project window

        string scriptCommandContent = $@"
using _1_Game.Scripts.Util;
using Cysharp.Threading.Tasks;
using Game.Systems.UI;

namespace Game.UI
{{
    public class Open{popupName}Command : ICommand
    {{
        public async UniTask Execute()
        {{
            Locator<{popupName}Provider>.Set(new {popupName}Provider());
            await Locator<UISystem>.Instance.Show<{popupName}>();
            Locator<{popupName}Provider>.Release();
            await UniTask.Yield();
        }}
    }}
}}
";
        string scriptCommandPath = $"Assets/1_Game/Scripts/UI/{popupName}/Open{popupName}Command.cs";
        await File.WriteAllTextAsync(scriptCommandPath, scriptCommandContent);
        AssetDatabase.Refresh(); // Refresh the AssetDatabase so the new script appears in the Project window
        
        Debug.Log("Scripts created successfully!");

        //await UniTask.Delay(TimeSpan.FromSeconds(2));
        Debug.Log("Creating prefab...");

// 2. Create the prefab
        string prefabPath = $"Assets/1_Game/Resources/UI/{popupName}/{popupName}.prefab";
        if (!Directory.Exists($"Assets/1_Game/Resources/UI/{popupName}"))
        {
            Directory.CreateDirectory($"Assets/1_Game/Resources/UI/{popupName}");
        }

        if (File.Exists(prefabPath))
        {
            EditorUtility.DisplayDialog("Error", "Prefab already exists!", "OK");
            return;
        }

// Create a GameObject with RectTransform
        GameObject popupPrefab = new GameObject(popupName, typeof(RectTransform));
        RectTransform rectTransform = popupPrefab.GetComponent<RectTransform>();

// Configure RectTransform defaults
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);

// Save as a prefab
        PrefabUtility.SaveAsPrefabAsset(popupPrefab, prefabPath);
        DestroyImmediate(popupPrefab); // Clean up the temporary GameObject
        Debug.Log("Prefab created successfully!");

// Notify the user
        EditorUtility.DisplayDialog("Success", $"Popup {popupName} has been created!", "OK");

// Refresh the AssetDatabase
        AssetDatabase.Refresh();
    }
}