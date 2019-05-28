using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour {

    public canvasElement [] canvasElements;
    public string CanvasElementActive;

    public enum statusCanvasMenu
    {
       main_canvas,
       trainingPath_canvas,
       options_canvas
    };
    public statusCanvasMenu statusMenu;
    // Use this for initialization
    void Start () {
        canvasElements = GameObject.FindGameObjectWithTag("mainUI").GetComponentsInChildren<canvasElement>();
	}



    public void checkCanvasElement_Status() {
        foreach (canvasElement canvas in canvasElements){
            if (canvas.isActive && canvas.isInteractable && canvas.canBlockRaycast) {
             CanvasElementActive = canvas.nameCanvasElement.ToString();           
            }
        }
    }
    
    public void changeEnumState(string state){
        switch (state) {
            case "Main": {
                    statusMenu = statusCanvasMenu.main_canvas;
                    break;
            }
            case "PathLearning": {
                    statusMenu = statusCanvasMenu.trainingPath_canvas;
                    break;
                }
            case "Options":{
                    statusMenu = statusCanvasMenu.options_canvas;
                    break;
                }

        }
    }

    // Update is called once per frame
    void Update () {

        checkCanvasElement_Status();
        changeEnumState(CanvasElementActive);

    }
 
}
