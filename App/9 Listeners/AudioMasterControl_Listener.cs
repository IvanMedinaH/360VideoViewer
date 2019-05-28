using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class AudioMasterControl_Listener : MonoBehaviour
{
    [Range(0,1)]
    public float slider;

    private UnityAction AudMaster_listener;

    // Use this for initialization
    void Start()
    {

    }

    private void OnEnable()
    {
        EventManager.StartListening("upVolume", upVolume);
    }

    private void OnDisable()
    {
        EventManager.StartListening("downVolume", downVolume);
    }

    public void downVolume(){

    }

    public void upVolume(){

    }

    
    // Update is called once per frame
    void Update()
    {

    }
}
