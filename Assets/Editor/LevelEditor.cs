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
    private string[] itemTypesText = new[] { "Erase", "Ground", "Border", "CubeGreen", "CubeRed", "CubeYellow"};
    private int selectedItem = 0;

    private void OnEnable()
    {
        level = target as SOLevel;
        width = level.Width;
        height = level.Height;
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
        level.Width = width;
        level.Height = height;

        DrawButtons();
        
        EditorGUILayout.Space(20);
        EditorGUILayout.HelpBox(helpBoxText, MessageType.None);
        EditorUtility.SetDirty(level);
    }
    
    void DrawButtons()
    {
        if (GUILayout.Button("Draw Width x Height Ground"))
            CreateGround();
        
        selectedItem = GUILayout.Toolbar(selectedItem, itemTypesText);
        
        EditorGUILayout.Space(20); GUILayout.Label("Clear", EditorStyles.boldLabel); EditorGUILayout.Space();
        if (GUILayout.Button("Remove Last"))
            RemoveAt(mousePosition);
        if (GUILayout.Button("Clear"))
            Clear();
    }
    
    private void Draw3DObjectOnScene()
    {
        for (int i = 0; i < level.levelGrid.Count; i++)
        {
            switch (level.levelGrid[i].type)
            {
                case ItemTypes.Ground:
                    Handles.color = Color.white;
                    Handles.DrawWireCube(level.levelGrid[i].position, new Vector3(1,0f,1));
                    break;
                case ItemTypes.Border:
                    Handles.color = Color.black;
                    Handles.DrawWireCube(level.levelGrid[i].position, Vector3.one);
                    break;
                case ItemTypes.CubeGreen:
                    Handles.color = Color.green;
                    Handles.DrawWireCube(level.levelGrid[i].position, Vector3.one);
                    break;
                case ItemTypes.CubeRed:
                    Handles.color = Color.red;
                    Handles.DrawWireCube(level.levelGrid[i].position, Vector3.one);
                    break;
                case ItemTypes.CubeYellow:
                    Handles.color = Color.yellow;
                    Handles.DrawWireCube(level.levelGrid[i].position, Vector3.one);
                    break;
            }
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
        ItemTypes tempItemType = ItemTypeConvert(itemTypesText[selectedItem]);
        if (tempItemType == ItemTypes.Null)
            return;
        
        LevelItem levelItem = new LevelItem(tempItemType, position);
        level.levelGrid.Add(levelItem);
    }
    
    private void CreateGround()
    {
        level.levelGrid.Clear();
        for (var i = 0; i < level.Width; i++)
        {
            var position = Vector3.zero;

            for (var j = 0; j < level.Height; j++)
            {
                position = new Vector3(i, 0f, j);
                selectedItem = 1;
                CreateObject(position);
                
                //For Borders
                if (j == 0)
                {
                    selectedItem = 2;
                    position = new Vector3(i, 0f, j-1);
                    CreateObject(position);
                }
                
                if (j == level.Height - 1)
                {
                    selectedItem = 2;
                    position = new Vector3(i, 0f, j+1);
                    CreateObject(position);
                }

                if (i == 0)
                {
                    selectedItem = 2;
                    position = new Vector3(i - 1, 0f, j);
                    CreateObject(position);
                }
                
                if (i == level.Width - 1)
                {
                    selectedItem = 2;
                    position = new Vector3(i + 1, 0f, j);
                    CreateObject(position);
                }
            }

            
            
            
            
        }
    }
    
    private void RemoveAt(Vector3 position)
    {
        for (int i = 0; i < level.levelGrid.Count; i++)
        {
            if (level.levelGrid[i].position == position)
            {
                level.levelGrid.RemoveAt(i);
                break;
            }
        }
    }

    private void Clear()
    {
        ItemTypes tempItemType = ItemTypeConvert(itemTypesText[selectedItem]);
        if (tempItemType == ItemTypes.Null)
            return;

        for (int i = 0; i < level.levelGrid.Count; i++)
        {
            if (level.levelGrid[i].type == tempItemType)
            {
                level.levelGrid.RemoveAt(i);
                i--;
            }
        }
    }
    
    private ItemTypes ItemTypeConvert(string text)
    {
        switch (text)
        {
            case "Ground":
                return ItemTypes.Ground; 
            case "Border":
                return ItemTypes.Border;
            case "CubeGreen":
                return ItemTypes.CubeGreen;
            case "CubeRed":
                return ItemTypes.CubeRed;
            case "CubeYellow":
                return ItemTypes.CubeYellow;
        }

        return ItemTypes.Null;
    }
    
    
    
}
#endif