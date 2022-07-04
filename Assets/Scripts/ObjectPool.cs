using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;
    [SerializeField] private List<PoolItem> poolItems;
    private Dictionary<ItemTypes, List<GameObject>> pools;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        pools = new Dictionary<ItemTypes, List<GameObject>>();
        for (int i = 0; i < poolItems.Count; i++)
        {
            List<GameObject> pooledObjects = new List<GameObject>();
            GameObject tempItem;
            for (int j = 0; j < poolItems[i].Amount; j++)
            {
                tempItem = Instantiate(poolItems[i].Prefab, transform, true);
                tempItem.SetActive(false);
                pooledObjects.Add(tempItem);
            }
            
            pools.Add(poolItems[i].Type, pooledObjects);
        }
    }
    
    public GameObject GetPooledObject(ItemTypes type, Vector3 position, Vector3 rotation)
    {
        for(int i = 0; i < pools[type].Count; i++)
        {
            if(!pools[type][i].activeInHierarchy)
            {
                pools[type][i].transform.position = position;
                pools[type][i].transform.eulerAngles = rotation;
                pools[type][i].SetActive(true);
                return pools[type][i];
            }
        }
        return null;
    }
    
    public void PutInPool(GameObject obj)
    {
        obj.SetActive(false);
    }
    
}


[System.Serializable]
public struct PoolItem
{
    [SerializeField] ItemTypes type;
    [SerializeField] GameObject prefab;
    [SerializeField] int amount;
    
    public ItemTypes Type { get => type; set => type = value; }
    public GameObject Prefab { get => prefab; set => prefab = value; }
    public int Amount { get => amount; set => amount = value; }
}

[System.Serializable]
public enum ItemTypes
{
    Null,
    Ground,
    Border,
    CubeGreen,
    CubeRed,
    CubeYellow
}