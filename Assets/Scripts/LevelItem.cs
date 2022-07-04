using UnityEngine;

[System.Serializable]
public class LevelItem
{
    public ItemTypes type;
    public Vector3 position;

    public LevelItem(ItemTypes type, Vector3 position)
    {
        this.type = type;
        this.position = position;
    }
}

