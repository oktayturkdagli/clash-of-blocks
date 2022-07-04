using UnityEngine;

public class CubeManager : MonoBehaviour
{
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
        Debug.Log("I am " + gameObject.tag + " I will Spread!!!");
    }
}
