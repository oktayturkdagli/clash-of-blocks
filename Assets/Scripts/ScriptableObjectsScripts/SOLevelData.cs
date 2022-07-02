using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Level Data", fileName = "New Level Data")]
public class SOLevelData : ScriptableObject
{
    [SerializeField] private SOLevel[] levels;
    [SerializeField] private int levelIndex = 1;

    public void DrawLevel()
    {
        if (levelIndex > levels.Length)
            levelIndex = Random.Range(1, levels.Length + 1);
        
        levels[levelIndex - 1].CreateLevel();
    }

    public Vector3 GetCameraPosition()
    {
        float x = 0f, y = 6, z = -2f;
        x += levels[levelIndex - 1].width * 0.45f;
        y += levels[levelIndex - 1].height * 2f;
        return new Vector3(x, y, z);
    }
    
}