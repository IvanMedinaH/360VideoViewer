using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class UI_VideoInfoPos_listener : MonoBehaviour
{
    private UnityAction Input_listener;
    public int selectedPos;
    public Input_RecipientPosController posControl;
    public Positions currentPosition;

    void Start(){       
        posControl = GameObject.FindGameObjectWithTag("AppManager").GetComponent<Input_RecipientPosController>();
        currentPosition = GetComponent<Positions>();
    }


    private void OnEnable(){
        EventManager.StartListening("changeUI_Position", changeUI_Position);
    }

    private void OnDisable(){
        EventManager.StopListening("changeUI_Position", changeUI_Position);
    }


    public void changeUI_Position(){
        selectedPos = posControl.currentPosition;
        changeIndicatorPositionTo(selectedPos);
    }

    public void changeIndicatorPositionTo(int pos) {       
       this.gameObject.transform.position = currentPosition.positions[pos];
    }

}
