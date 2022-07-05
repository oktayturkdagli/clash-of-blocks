using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "Scriptable Objects/Level", fileName = "New Level")]
public class SOLevel : ScriptableObject
{
    public List<LevelItem> levelItems = new List<LevelItem>();

    public int width = 5;
    public int height = 5;

    public void CreateLevel()
    {
        levelItems = levelItems.OrderBy(x => x.position.x).ThenBy(x => x.position.z).ToList();
        for (int i = 0; i < levelItems.Count; i++)
        {
            GameObject item = ObjectPool.SharedInstance.GetPooledObject(levelItems[i].type, levelItems[i].position, Vector3.zero);
        }
    }
    
}