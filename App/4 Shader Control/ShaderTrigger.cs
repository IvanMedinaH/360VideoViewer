using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TriggerUtilities))]
public class ShaderTrigger : MonoBehaviour
{
    public TriggerUtilities utilities;


    // Use this for initialization
    void Start(){
        utilities = GetComponent<TriggerUtilities>();
    }

    // Update is called once per frame
    void Update(){

        if (Input.GetKey(KeyCode.Return))
        {
            EventManager.TriggerEvent("disintegrate");
        }
        if (Input.GetKey(KeyCode.Space))
        {
            EventManager.TriggerEvent("Integrate");
        }

        if (Input.GetKey(KeyCode.P)) {
            utilities.intBnaryValue();
        }

    }




    

}
