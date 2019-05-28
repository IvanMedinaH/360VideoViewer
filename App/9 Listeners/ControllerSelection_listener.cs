using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControllerSelection_listener : MonoBehaviour {
    [Header("Position control")]
    public Vector3 currenPosition;
    public Vector3 NextPosition;
    public Vector3 LastPosition;
    public Positions positions;

    private UnityAction actionListener;
    Input_RecipientPosController posController;

    // Use this for initialization
    void Start () {
      posController = GameObject.FindGameObjectWithTag("AppManager").GetComponent<Input_RecipientPosController>();
      positions = GetComponent<Positions>();     
	}

    private void OnEnable()
    {
        EventManager.StartListening("goNextPosition", goNextPosition);
        EventManager.StartListening("goPreviusPosition", goPreviusPosition);
    }

    private void OnDisable()
    {
        EventManager.StopListening("goNextPosition",goNextPosition);
        EventManager.StopListening("goPreviousPosition", goPreviusPosition);
    }


    public int setCurrenPosition(int pos){
        int temp_pos = pos;
        return temp_pos;
    }

    public void goNextPosition() {
        Debug.Log("move to next element");
    }

    public void goPreviusPosition() {
        Debug.Log("move to previous element");
    }

  



}
