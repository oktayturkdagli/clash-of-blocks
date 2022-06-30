using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        EventManager.current.onStartGame += OnStartGame;
        EventManager.current.onFinishGame += OnFinishGame;
        EventManager.current.OnStartGame();
    }

    void OnStartGame()
    {
        Debug.Log("Game is START!");
        Invoke(nameof(LateStart), 1f); // Game starts 1 second late to wait for all classes to load correctly
    }

    void OnFinishGame()
    {
        Debug.Log("Game is OVER!");
    }

  
    void LateStart()
    {
        
    }

    void OnDestroy()
    {
        EventManager.current.onStartGame -= OnStartGame;
        EventManager.current.onFinishGame -= OnFinishGame;
    }
}
