using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

public class EventManager : MonoBehaviour
{
    [Header("APPManager")]

    private Dictionary<string, UnityEvent> eventDictionary;

    private static EventManager eventManager;

    #region Singleton EventManager
    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("se requiere un EventManger activo script en un GameObject en la scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }
    #endregion

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }
    }

    #region
    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }
    #endregion

    #region
    public static void StopListening(string eventName, UnityAction listener)
    {
        if (eventManager == null) return;
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }
    #endregion

    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }

    public static void TriggerParameterEvent(string eventName, string [] parameter) {
        Type myType = eventName.GetType();
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
            var loadingMethod = thisEvent.GetType().GetMethod(eventName + myType);
            loadingMethod.Invoke(eventName, parameter);
          // thisEvent.
          //  thisEvent.Invoke();
        }
    }
}