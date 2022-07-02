using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;
    [SerializeField] private List<PoolItem> poolItems;
    private Dictionary<string, List<GameObject>> pools;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        pools = new Dictionary<string, List<GameObject>>();
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
            
            pools.Add(poolItems[i].Name, pooledObjects);
        }
    }
    
    public GameObject GetPooledObject(string name, Vector3 position, Vector3 rotation)
    {
        for(int i = 0; i < pools[name].Count; i++)
        {
            if(!pools[name][i].activeInHierarchy)
            {
                pools[name][i].transform.position = position;
                pools[name][i].transform.eulerAngles = rotation;
                pools[name][i].SetActive(true);
                return pools[name][i];
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
    [SerializeField] string name;
    [SerializeField] GameObject prefab;
    [SerializeField] int amount;
    
    public string Name { get => name; set => name = value; }
    public GameObject Prefab { get => prefab; set => prefab = value; }
    public int Amount { get => amount; set => amount = value; }
}