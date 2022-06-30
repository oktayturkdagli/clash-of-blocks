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


    //Events cannot be triggered directly from another class so they are triggered via functions
    public void OnStartGame()
    {
        onStartGame?.Invoke();
    }

    public void OnFinishGame()
    {
        onFinishGame?.Invoke();
    }
}

//ENUMs are used for ease of naming when transferring data
public enum DataType
{
    PlayerData
}

