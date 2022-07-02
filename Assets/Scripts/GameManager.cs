using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] SOLevelData levelData;

    private void Start()
    {
        levelData.DrawLevel();
        if (Camera.main != null) Camera.main.transform.position = levelData.GetCameraPosition();
        EventManager.current.OnStartGame();
    }

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

    void OnStartGame()
    {
        // Debug.Log("Game is START!");
    }

    void OnFinishGame()
    {
        // Debug.Log("Game is OVER!");
    }
    
}
