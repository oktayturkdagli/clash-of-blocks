using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Levels Data", fileName = "New Levels Data")]
public class SOLevelsData : ScriptableObject
{
    [SerializeField] private SOLevel[] levels;
    [SerializeField] private int levelIndex = 1;

    public int LevelIndex { get => levelIndex; set => levelIndex = value; }

    public void DrawLevel()
    {
        if (levelIndex > levels.Length)
            levelIndex = Random.Range(1, levels.Length + 1);
        
        levels[levelIndex - 1].CreateLevel();
    }

    public Vector3 GetCameraPosition()
    {
        float x = 0f, y = 6, z = -2f;
        x += levels[levelIndex - 1].Width * 0.45f;
        y += levels[levelIndex - 1].Width * 2f;
        z += levels[levelIndex - 1].Height * 0.1f;
        return new Vector3(x, y, z);
    }

    public SOLevel GetLevel()
    {
        return levels[levelIndex - 1];
    }
    
    public int GetLevelCount()
    {
        return levels.Length;
    }

}