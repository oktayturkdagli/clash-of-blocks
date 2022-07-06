using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float animationDuration = 1f;
    private readonly Dictionary<ItemTypes, Queue<LevelItem>> queues = new Dictionary<ItemTypes, Queue<LevelItem>>();
    private readonly Vector3[] directions = { Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
    private int itemTypesCount = 0;

    private void OnEnable()
    {
        EventManager.current.onStartSpread += OnStartSpread;
        itemTypesCount = Enum.GetNames(typeof(ItemTypes)).Length;
        for (int i = 3; i < itemTypesCount; i++)
        {
            queues.Add((ItemTypes)i, new Queue<LevelItem>());
        }
    }

    private void OnDisable()
    {
        EventManager.current.onStartSpread -= OnStartSpread;
    }

    private void OnStartSpread()
    {
        FindStartItemsAndAddQueue();
        HandleQueue();
    }

    private void FindStartItemsAndAddQueue()
    {
        // Objects in the scene are adding to the queue
        for (var i = 0; i < gameManager.levelGrid.Count; i++)
        {
            for (var j = 3; j < itemTypesCount; j++)
            {
                if (gameManager.levelGrid[i].type == (ItemTypes)j)
                {
                    queues[(ItemTypes)j].Enqueue(gameManager.levelGrid[i]);
                }
            }
        }
    }
    
    // When the placement are finished, return to this method again.
    private void HandleQueue()
    {
        int biggestQueueLength = 0;
        for (int i = 0; i < queues.Count; i++)
        {
            Queue<LevelItem> itemTypeQueue = queues.ElementAt(i).Value;
            int tempQueueCount = itemTypeQueue.Count;
            for (int j = 0; j < tempQueueCount; j++)
            {
                LevelItem item = itemTypeQueue.Dequeue();
                List<LevelItem> emptyNeighbors = FindEmptyNeighbors(item); 
                PlaceCubes(item, emptyNeighbors, itemTypeQueue);
            }

            if (itemTypeQueue.Count > biggestQueueLength)
                biggestQueueLength = itemTypeQueue.Count;
        }

        if (biggestQueueLength > 0)
        {
            StartCoroutine(Waiter(animationDuration / 5));
        }
        else
        {
            EventManager.current.OnEndSpread(); // Go GameManager
        }
    }
    
    private List<LevelItem> FindEmptyNeighbors(LevelItem item)
    {
        var emptyNeighbors = new List<LevelItem>();
        for (int i = 0; i < directions.Length; i++)
        {
            var neighborPosition = item.position + directions[i];
            var neighbor = gameManager.levelGrid.FirstOrDefault(x => x.position == neighborPosition);
            if (neighbor != null && neighbor.type == ItemTypes.Ground)
            {
                emptyNeighbors.Add(neighbor);
            }
        }
        
        return emptyNeighbors;
    }

    private void PlaceCubes(LevelItem item, List<LevelItem> emptyNeighbors, Queue<LevelItem> waitingQueue)
    {
        ItemTypes convertThisType = item.type;
        List<GameObject> newObjects = new List<GameObject>();
        for (var i = 0; i < emptyNeighbors.Count; i++)
        {
            LevelItem neighbor = emptyNeighbors[i];
            
            //On Grid
            gameManager.levelGrid.Remove(neighbor);
            var newLevelItem = new LevelItem(convertThisType, neighbor.position);
            gameManager.levelGrid.Add(newLevelItem);
            waitingQueue.Enqueue(newLevelItem);
        
            //On Scene
            var newObject = ObjectPool.SharedInstance.GetPooledObject(convertThisType, neighbor.position, Vector3.zero);
            newObjects.Add(newObject);
        }
        
        PlayPlacementAnimations(item, newObjects, animationDuration);
    }

    private void PlayPlacementAnimations(LevelItem item, List<GameObject> newObjects, float animationTime)
    {
        for (int i = 0; i < newObjects.Count; i++)
        {
            var originalPos = newObjects[i].transform.position;
            var animationStartPos = item.position;
            var originalrot = newObjects[i].transform.eulerAngles; 
            var animationStartRot = Vector3.zero;
            if (newObjects[i].transform.position.x > item.position.x)
            {
                animationStartRot = new Vector3(0, 0, 90f);
            }
            else if (newObjects[i].transform.position.x < item.position.x)
            {
                animationStartRot = new Vector3(0, 0, -90f);
            }
            else if (newObjects[i].transform.position.z > item.position.z)
            {
                animationStartRot = new Vector3(-90, 0, 0);
            }
            else
            {
                animationStartRot = new Vector3(90, 0, 0);
            }

            newObjects[i].transform.position = animationStartPos;
            newObjects[i].transform.eulerAngles = animationStartRot;
            newObjects[i].transform.DOMove(originalPos, animationTime).SetId("Placement");
            newObjects[i].transform.DORotate(originalPos, animationTime).SetId("Placement");
        }
    }

    private IEnumerator Waiter(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        HandleQueue();
    }

}
