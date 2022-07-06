using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager current;

    private void Awake()
    {
        current = this;
    }

    //Events are created
    public event Action onStartGame;
    public event Action onFinishGame;
    public event Action onWinGame;
    public event Action onLoseGame;
    public event Action onStartSpread;
    public event Action onEndSpread;
    
    public event Action onLastDanceTriggered;
    
    
    //Events cannot be triggered directly from another class so they are triggered via functions
    public void OnStartGame()
    {
        onStartGame?.Invoke();
    }

    public void OnFinishGame()
    {
        onFinishGame?.Invoke();
    }
    
    public void OnWinGame()
    {
        onWinGame?.Invoke();
    }
    
    public void OnLoseGame()
    {
        onLoseGame?.Invoke();
    }
    
    public void OnStartSpread()
    {
        onStartSpread?.Invoke();
    }
    
    public void OnEndSpread()
    {
        onEndSpread?.Invoke();
    }
    
    public void OnLastDanceTriggered()
    {
        onLastDanceTriggered?.Invoke();
    }
    
    
}

