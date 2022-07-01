using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Level Data", fileName = "New Level Data")]
public class SOLevelData : ScriptableObject
{
    [SerializeField] private SOLevel[] levels;
    private int levelIndex = 1;

    private void OnDisable()
    {
        ResetLevelIndex();
    }
    
    public void IncreaseLevelIndex()
    {
        if (levelIndex == levels.Length)
            levelIndex = 0;
        levelIndex++;
    }

    public void DrawLevel()
    {
        if (levelIndex > levels.Length)
            levelIndex = Random.Range(1, levels.Length + 1);
        
        levels[levelIndex - 1].CreateLevel();
    }
    
    private void ResetLevelIndex()
    {
        levelIndex = 1;
    }
    
    public int GetLevelIndex()
    {
        return levelIndex;
    }

    public Vector3 GetCameraPosition()
    {
        float x = Mathf.FloorToInt(levels[levelIndex - 1].width / 2f);
        float y = levels[levelIndex - 1].width * 2f;
        return new Vector3(x, y, -1f);
    }
}