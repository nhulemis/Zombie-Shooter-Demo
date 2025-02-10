using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class HierarchyButton
{
    private static Dictionary<int,string> Dictionary = new Dictionary<int, string>();
    
    public static int SceneIndex = 0;
    static HierarchyButton()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
    }
    
    private static string GetSceneNameFromInstanceId(int instanceId)
    {
        // Loop through all loaded scenes
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.handle == instanceId)
            {
                // Return the scene name if the instanceId matches its handle
                return scene.path;
            }
        }
        return null; // Not a scene header
    }

    private static void OnHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        Rect buttonRect = new Rect(selectionRect.x + selectionRect.width - 40, selectionRect.y, 40, selectionRect.height);
        Object obj = EditorUtility.InstanceIDToObject(instanceID);
        if (obj is null)
        {
            string scenePath = GetSceneNameFromInstanceId(instanceID);
            if (GUI.Button(buttonRect, "Load"))
            {
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
            }
            
            if(string.IsNullOrEmpty(scenePath)) return;
            
            if(!scenePath.Contains("Boostrap")) return;
            Rect unloadRect = new Rect(selectionRect.x + selectionRect.width - 2 * 40, selectionRect.y, 40, selectionRect.height);

            if (GUI.Button(unloadRect, "~All"))
            {
                UnloadAllSceneDontRemoveOutOfHierarchy();
            }
            
            Rect setActiveScene = new Rect(selectionRect.x + selectionRect.width - 3*40, selectionRect.y, 40, selectionRect.height);
            if (GUI.Button(setActiveScene, "*All"))
            {
                LoadAllScenesFromBuildSetting();
            }
        }

        

    }

    private static void UnloadAllSceneDontRemoveOutOfHierarchy()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            if (scene.name != "Boostrap")
            {
                EditorSceneManager.CloseScene(scene,false);
            }
        }
    }

    private static void LoadAllScenesFromBuildSetting()
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            if(scenePath.Contains("Log")) continue;
            Debug.Log(scenePath);
            var x = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
            EditorSceneManager.CloseScene(x,false);
        }
    }
}