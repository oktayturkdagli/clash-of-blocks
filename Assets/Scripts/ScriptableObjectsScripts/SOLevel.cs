using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "Scriptable Objects/Level", fileName = "New Level")]
public class SOLevel : ScriptableObject
{
    public List<LevelItem> levelGrid = new List<LevelItem>();

    private int width = 5;
    private int height = 5;
    public int Width { get => width; set => width = value; }
    public int Height { get => height; set => height = value; }
    
    public void CreateLevel()
    {
        levelGrid = levelGrid.OrderBy(x => x.position.x).ThenBy(x => x.position.z).ToList();
        for (int i = 0; i < levelGrid.Count; i++)
        {
            GameObject item = ObjectPool.SharedInstance.GetPooledObject(levelGrid[i].type, levelGrid[i].position, Vector3.zero);
        }
    }
    
}