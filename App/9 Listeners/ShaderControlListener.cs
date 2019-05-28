using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShaderControlListener : MonoBehaviour {


    private UnityAction shader_listener;
    ShaderControl control;

    private void Start()
    {
     control = GetComponent<ShaderControl>();
    }


    private void OnEnable()
    {
        EventManager.StartListening("disintegrate", Material_disintegrator);
        EventManager.StartListening("Integrate", Material_Reintegrator);
    }


    private void OnDisable()
    {
        EventManager.StopListening("disintegrate", Material_disintegrator);
        EventManager.StopListening("Integrate", Material_Reintegrator);
    }



    public void Material_disintegrator() {
        control.canDisintegrate = true;
    }


    public void Material_Reintegrator(){
        control.canReintegrate = true;
        control.Reset();
    }

    



}
