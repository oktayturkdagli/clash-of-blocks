using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "Scriptable Objects/Level", fileName = "New Level")]
public class SOLevel : ScriptableObject
{
    public List<Vector3> GroundPositions = new List<Vector3>();
    public List<Vector3> BorderPositions = new List<Vector3>();
    public List<Vector3> CubeGreenPositions = new List<Vector3>();
    public List<Vector3> CubeRedPositions = new List<Vector3>();
    public List<Vector3> CubeYellowPositions = new List<Vector3>();

    public int width = 5;
    public int height = 5;

    public void CreateLevel()
    {
        Dictionary<string, List<Vector3>> dictionary = new Dictionary<string, List<Vector3>>();
        dictionary.Add("Ground", GroundPositions);
        dictionary.Add("Border", BorderPositions);
        dictionary.Add("CubeGreen", CubeGreenPositions);
        dictionary.Add("CubeRed", CubeRedPositions);
        dictionary.Add("CubeYellow", CubeYellowPositions);
        
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