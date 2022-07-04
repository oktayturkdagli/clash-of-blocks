using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "Scriptable Objects/Level", fileName = "New Level")]
public class SOLevel : ScriptableObject
{
    public List<Vector3> groundPositions = new List<Vector3>();
    public List<Vector3> borderPositions = new List<Vector3>();
    public List<Vector3> cubeGreenPositions = new List<Vector3>();
    public List<Vector3> cubeRedPositions = new List<Vector3>();
    public List<Vector3> cubeYellowPositions = new List<Vector3>();

    public int width = 5;
    public int height = 5;

    public void CreateLevel()
    {
        Dictionary<string, List<Vector3>> dictionary = new Dictionary<string, List<Vector3>>();
        dictionary.Add("Ground", groundPositions);
        dictionary.Add("Border", borderPositions);
        dictionary.Add("CubeGreen", cubeGreenPositions);
        dictionary.Add("CubeRed", cubeRedPositions);
        dictionary.Add("CubeYellow", cubeYellowPositions);
        
        for (int i = 0; i < dictionary.Count; i++)
        {
            string key = dictionary.ElementAt(i).Key;
            for (int j = 0; j < dictionary[key].Count; j++)
            {
                GameObject item = ObjectPool.SharedInstance.GetPooledObject(key, dictionary[key][j], Vector3.zero);
            }
        }
    }
    
}