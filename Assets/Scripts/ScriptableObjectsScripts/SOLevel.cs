using UnityEngine;
using System.Collections.Generic;

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
        
    }
    
}