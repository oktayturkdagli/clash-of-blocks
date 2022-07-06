using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Lean.Touch;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] public SOLevelsData levelsData;
    private List<LevelItem> playerCubes = new List<LevelItem>();
    private int playerCubeCounter = 0;
    public List<LevelItem> levelGrid;
    private SOLevel level;
    private Camera cam;
    private bool canTouch = false;

    private void OnEnable()
    {
        EventManager.current.onStartGame += OnStartGame;
        EventManager.current.onFinishGame += OnFinishGame;
        EventManager.current.onEndSpread += OnEndSpread;
    }

    private void OnDisable()
    {
        EventManager.current.onStartGame -= OnStartGame;
        EventManager.current.onFinishGame -= OnFinishGame;
    }
    
    private void Start()
    {
        level = levelsData.GetLevel();
        levelGrid = level.levelGrid;
        levelsData.DrawLevel();
        cam = Camera.main;
        if (cam != null) cam.transform.position = levelsData.GetCameraPosition();
        GameObject item = ObjectPool.SharedInstance.GetPooledObject(ItemTypes.CubeGreen, new Vector3(100,3,100), Vector3.zero);
        playerCubes.Add(new LevelItem(ItemTypes.CubeGreen, new Vector3(100,3,100)));
        EventManager.current.OnStartGame();
    }
    
    private void OnStartGame()
    {
        canTouch = true;
    }
    
    private void OnFinishGame()
    {
        //Do Nothing
    }
    
    private void OnEndSpread()
    {
        EventManager.current.OnLastDanceTriggered(); // Go UIManager
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
                    PlaceCube(hittedObj);
                }
            }
        }
    }

    private void PlaceCube(GameObject hittedObj)
    {
        canTouch = false;
        var hittedObjPosition = hittedObj.transform.position;
        var item = ObjectPool.SharedInstance.GetPooledObject(playerCubes[playerCubeCounter].type, new Vector3(hittedObjPosition.x, 3, hittedObjPosition.z), Vector3.zero);
        playerCubes[playerCubeCounter].position = hittedObjPosition; // For Grid
        levelGrid.Remove(levelGrid.FirstOrDefault(x => x.position == hittedObj.transform.position)); //Remove the touched Ground
        levelGrid.Add(playerCubes[playerCubeCounter]); //Remove this instead
        item.transform.DOMoveY(0, 1f).OnComplete(() =>
        {
            playerCubeCounter++;
            if (playerCubeCounter > playerCubes.Count - 1)
            {
                EventManager.current.OnStartSpread(); // Go CubeManager
            }
            else
            {
                canTouch = false;
            }
        });
    }
    
}
