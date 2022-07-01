#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SOLevel))]
public class LevelEditor : Editor
{
    private SOLevel level;
    Vector3 mousePosition;
    private int width = 0, height = 0;
    private const string helpBoxText =
        "\nUsing this custom editor you can design your grid based level.\n" +
        "\nClear button deletes all objects of the specified type.\n" + 
        "\nErase option deletes the objects you specify with the pointer\n";

    private bool onMouseDown = false;
    private string[] itemTypes = new[] { "Erase", "Ground", "Border", "CubeGreen", "CubeRed", "CubeYellow"};
    private int selectedItem = 0;

    private void OnEnable()
    {
        level = target as SOLevel;
        width = level.width;
        height = level.height;
        SceneView.duringSceneGui += OnScene;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnScene;
    }
    
    private void OnScene(SceneView sceneView)
    {
        CheckInput();
        HandleInput();
        Draw3DObjectOnScene();
    }
    
    public override void OnInspectorGUI()
    {
        DrawMyInspector();
    }

    void DrawMyInspector()
    {
        EditorGUILayout.Space(); GUILayout.Label("Create Your Level", EditorStyles.boldLabel); EditorGUILayout.Space();
        EditorGUILayout.Space(); GUILayout.Label("Draw", EditorStyles.boldLabel); EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Width", EditorStyles.boldLabel);
        width = EditorGUILayout.IntField(width);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Height", EditorStyles.boldLabel);
        height = EditorGUILayout.IntField(height);
        GUILayout.EndHorizontal();
        level.width = width;
        level.height = height;

        DrawButtons();
        
        EditorGUILayout.Space(20);
        EditorGUILayout.HelpBox(helpBoxText, MessageType.None);
        EditorUtility.SetDirty(level);
    }
    
    void DrawButtons()
    {
        if (GUILayout.Button("Draw Width x Height Ground"))
            CreateGround();
        
        selectedItem = GUILayout.Toolbar(selectedItem, itemTypes);
        
        EditorGUILayout.Space(20); GUILayout.Label("Clear", EditorStyles.boldLabel); EditorGUILayout.Space();
        if (GUILayout.Button("Remove Last"))
            RemoveAt(mousePosition);
        if (GUILayout.Button("Clear"))
            Clear();
    }
    
    private void Draw3DObjectOnScene()
    {
        Handles.color = Color.white;
        for (int i = 0; i < level.GroundPositions.Count; i++)
        {
            Handles.DrawWireCube(level.GroundPositions[i], new Vector3(1,0f,1));
        }
        Handles.color = Color.black;
        for (int i = 0; i < level.BorderPositions.Count; i++)
        {
            Handles.DrawWireCube(level.BorderPositions[i], Vector3.one);
        }
        Handles.color = Color.green;
        for (int i = 0; i < level.CubeGreenPositions.Count; i++)
        {
            Handles.DrawWireCube(level.CubeGreenPositions[i], Vector3.one);
        }
        Handles.color = Color.red;
        for (int i = 0; i < level.CubeRedPositions.Count; i++)
        {
            Handles.DrawWireCube(level.CubeRedPositions[i], Vector3.one);
        }
        Handles.color = Color.yellow;
        for (int i = 0; i < level.CubeYellowPositions.Count; i++)
        {
            Handles.DrawWireCube(level.CubeYellowPositions[i], Vector3.one);
        }
    }
    
    private void CheckInput()
    {
        Event guiEvent = Event.current;
        if (guiEvent.type == EventType.Layout)
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive)); // When the new object is created, Don't assign the inspector to that object 
        else if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0) //When Down the left mouse button
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
            float drawPlaneHeight = 0;
            float distanceToDrawPlane = (drawPlaneHeight - ray.origin.y) / ray.direction.y;
            mousePosition = ray.GetPoint(distanceToDrawPlane);
            mousePosition.x = Mathf.Round(mousePosition.x);
            mousePosition.z = Mathf.Round(mousePosition.z);
            onMouseDown = true;
        }
        else
        {
            onMouseDown = false;
        }
    }
    
    private void HandleInput()
    {
        if (onMouseDown)
        {
            onMouseDown = false;
            RemoveAt(mousePosition);
            CreateObject(mousePosition);
        }
    }
    
    private void CreateObject(Vector3 position)
    {
        switch (itemTypes[selectedItem])
        {
            case "Erase":
                break;
            case "Ground":
                level.GroundPositions.Add(position);
                break;
            case "Border":
                level.BorderPositions.Add(position);
                break;
            case "CubeGreen":
                level.CubeGreenPositions.Add(position);
                break;
            case "CubeRed":
                level.CubeRedPositions.Add(position);
                break;
            case "CubeYellow":
                level.CubeYellowPositions.Add(position);
                break;
        }
    }
    
    private void CreateGround()
    {
        for (var i = 0; i < level.width; i++)
        {
            for (var j = 0; j < level.height; j++)
            {
                var position = new Vector3(i, 0f, j);
                selectedItem = 1;
                RemoveAt(position);
                CreateObject(position);
            }
        }
    }
    
    private void RemoveAt(Vector3 position)
    {
        if (!(itemTypes[selectedItem].Equals("CubeRed") || itemTypes[selectedItem].Equals("CubeGreen") || itemTypes[selectedItem].Equals("CubeYellow")))
        {
            if (level.GroundPositions.Contains(position))
                level.GroundPositions.Remove(position);
        }
        
        if (level.BorderPositions.Contains(position))
            level.BorderPositions.Remove(position);
        
        if (level.CubeGreenPositions.Contains(position))
            level.CubeGreenPositions.Remove(position);
        
        if (level.CubeRedPositions.Contains(position))
            level.CubeRedPositions.Remove(position);
        
        if (level.CubeYellowPositions.Contains(position))
            level.CubeYellowPositions.Remove(position);
    }

    private void Clear()
    {
        switch (itemTypes[selectedItem])
        {
            case "Erase":
                break;
            case "Ground":
                level.GroundPositions.Clear();
                break;
            case "Border":
                level.BorderPositions.Clear();
                break;
            case "CubeGreen":
                level.CubeGreenPositions.Clear();
                break;
            case "CubeRed":
                level.CubeRedPositions.Clear();
                break;
            case "CubeYellow":
                level.CubeYellowPositions.Clear();
                break;
        }
    }
    
}
#endif