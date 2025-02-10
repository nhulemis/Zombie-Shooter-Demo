using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _1_Game.Scripts.Editor
{
    [InitializeOnLoad]
    public static class BeforePlay
    {
        static BeforePlay()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        private static void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                // Call your function here
                Debug.Log("Before Play Mode");
                YourFunction();
            }
        }

        private static void YourFunction()
        {
            
           // SceneManager.LoadScene("Boostrap",LoadSceneMode.Single);
        }
    }
}