#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using Object = UnityEngine.Object;

public class LevelEditor : EditorWindow
{
    [MenuItem("Window/Level Editor")]
    public static void Init()
    {
        EditorWindow editorWindow = GetWindow(typeof(LevelEditor), false, "Level Editor", true);
        editorWindow.minSize = new Vector2(350, 500);
        editorWindow.maxSize = new Vector2(350, 500);
    }

    void OnGUI()
    {
        DrawMyEditor();
    }

    void DrawMyEditor()
    {
        EditorGUILayout.Space(); GUILayout.Label("Create Object", EditorStyles.boldLabel); EditorGUILayout.Space();
    }
    
}
#endif