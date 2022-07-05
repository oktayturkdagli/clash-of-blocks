using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    private bool canSpread = true;
    private Queue<LevelItem> waitingQueue = new Queue<LevelItem>();
    private Vector3[] directions = { Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

    private void OnEnable()
    {
        EventManager.current.onStartSpread += OnStartSpread;
    }

    private void OnDisable()
    {
        EventManager.current.onStartSpread -= OnStartSpread;
    }

    private void OnStartSpread()
    {
        if (canSpread)
        {
            Debug.Log("I am spread");
            FindStartItemsAndAddQueue();
            HandleQueue();
        }
    }

    private void FindStartItemsAndAddQueue()
    {
        for (int i = 0; i < gameManager.levelGrid.Count; i++)
        {
            if (gameManager.levelGrid[i].type is ItemTypes.CubeGreen or ItemTypes.CubeRed or ItemTypes.CubeYellow)
            {
                waitingQueue.Enqueue(gameManager.levelGrid[i]);
            }
        }
    }
    
    // When the placement animations are finished, return to this method again.
    private void HandleQueue()
    {
        if (waitingQueue.Count > 0)
        {
            LevelItem item = waitingQueue.Dequeue();
            List<LevelItem> emptyNeighbors = FindEmptyNeighbors(item); 
            PlaceCubes(item, emptyNeighbors);
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

    private void PlaceCubes(LevelItem item, List<LevelItem> emptyNeighbors)
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

        if (emptyNeighbors.Count < 1)
        {
            StartCoroutine(Waiter(0f));
        }
        else
        {
            PlayPlacementAnimations(item, newObjects, 2f);
            StartCoroutine(Waiter(2f));
        }
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
