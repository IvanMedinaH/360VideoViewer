using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class EventTest : MonoBehaviour
{

    private UnityAction action_listener;

    void Awake()
    {
        action_listener = new UnityAction(funcion_1);
    }

    void OnEnable()
    {
        EventManager.StartListening("test", action_listener);
        EventManager.StartListening("Spawn", function_2);
        EventManager.StartListening("Destroy", function_3);
    }

    void OnDisable()
    {
        EventManager.StopListening("test", action_listener);
        EventManager.StopListening("Spawn", function_2);
        EventManager.StopListening("Destroy", function_3);
    }


    void funcion_1()
    {
        Debug.Log("Some Function was called!");
    }

    void function_2()
    {
        Debug.Log("Some Other Function was called!");
    }

    void function_3()
    {
        Debug.Log("Some Third Function was called!");
    }
}