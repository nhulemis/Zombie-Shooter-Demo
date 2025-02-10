using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.Runtime.CompilerServices;
public class CreateNewItemWindow : EditorWindow
{
    string itemName;
    System.Func<string,string> onEnter = null;

    string message = "";

    void OnGUI()
    {
        EditorGUILayout.PrefixLabel("Item Name");
        GUI.SetNextControlName("TextField");

        itemName = EditorGUILayout.TextField(itemName);

        if (!string.IsNullOrEmpty(message))
        {
            this.maxSize = new Vector2(215f, 80f);
            this.minSize = this.maxSize;
            EditorGUILayout.HelpBox(message, MessageType.Error);
        }

        if (Event.current.keyCode == KeyCode.Return && Event.current.type == EventType.KeyUp)
        {
            if (string.IsNullOrEmpty(itemName))
            {
                message = "Item ID can't be empty!!!";
                Repaint();
            }
            else
            {
                CreateNewItem(itemName);
            }
        }

        EditorGUI.FocusTextInControl("TextField");
        this.Focus();
    }

    void CreateNewItem(string itemName)
    {
        if (onEnter != null)
        {
            message = onEnter(itemName);
            if (string.IsNullOrEmpty(message))
            {
                Close();
            }
            else
            {
                Repaint();
            }
        }
        else
        {
            Close();
        }
    }

    
    public static void Show(System.Func<string,string> onEnter, string title = "Enter Name", string defaultValue = "Enter Value")
    {
        CreateNewItemWindow window = EditorWindow.GetWindow<CreateNewItemWindow>();
        window.titleContent = new GUIContent(title);
        window.itemName = defaultValue;
        window.onEnter = onEnter;
        window.maxSize = new Vector2(215f, 50f);
        window.minSize = window.maxSize;
        
        if (Event.current != null)
        {
            window.position = new Rect(GUIUtility.GUIToScreenPoint(Event.current.mousePosition - new Vector2(300, -100)), window.maxSize);
        }

        window.ShowUtility(); 
        window.Focus();
    }
}
#endif