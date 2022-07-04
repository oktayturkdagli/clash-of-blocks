using UnityEngine;
using System.Collections.Generic;
using Lean.Touch;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SOLevelData levelData;
    private Camera cam;
    private bool canTouch = false;
    private List<GameObject> playerCubes = new List<GameObject>();
    private int playerCubeCounter = 0;

    private void OnEnable()
    {
        EventManager.current.onStartGame += OnStartGame;
        EventManager.current.onFinishGame += OnFinishGame;
    }

    private void OnDisable()
    {
        EventManager.current.onStartGame -= OnStartGame;
        EventManager.current.onFinishGame -= OnFinishGame;
    }
    
    private void Start()
    {
        levelData.DrawLevel();
        cam = Camera.main;
        if (cam != null) cam.transform.position = levelData.GetCameraPosition();
        GameObject item = ObjectPool.SharedInstance.GetPooledObject("CubeGreen", new Vector3(100,3,100), Vector3.zero);
        playerCubes.Add(item);
        EventManager.current.OnStartGame();
    }
    
    private void OnStartGame()
    {
        canTouch = true;
    }

    public void OnDown(LeanFinger finger)
    {
        if (cam != null && canTouch)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, int.MaxValue))
            {
                GameObject hittedObj = hit.collider.gameObject;
                if (hittedObj.tag.Equals("Ground"))
                {
                    canTouch = false;
                    playerCubes[playerCubeCounter].transform.position = new Vector3(hittedObj.transform.position.x, 3, hittedObj.transform.position.z);
                    playerCubes[playerCubeCounter].transform.DOMoveY(0, 1f).OnComplete(() =>
                    {
                        playerCubeCounter++;
                        if (playerCubeCounter > playerCubes.Count - 1)
                        {
                            EventManager.current.OnStartSpread();
                        }
                        else
                        {
                            canTouch = true;
                        }
                    });
                }
                
            }
        }
    }


    void OnFinishGame()
    {
        // Debug.Log("Game is OVER!");
    }
    
}
