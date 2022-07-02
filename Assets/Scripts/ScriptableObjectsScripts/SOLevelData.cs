using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Level Data", fileName = "New Level Data")]
public class SOLevelData : ScriptableObject
{
    [SerializeField] private SOLevel[] levels;
    private int levelIndex = 1;

    public void DrawLevel()
    {
        if (levelIndex > levels.Length)
            levelIndex = Random.Range(1, levels.Length + 1);
        
        levels[levelIndex - 1].CreateLevel();
    }

    public Vector3 GetCameraPosition()
    {
        return new Vector3(4, 24, -2);
    }
    
}