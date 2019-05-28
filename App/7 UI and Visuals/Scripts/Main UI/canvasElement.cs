using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class canvasElement : MonoBehaviour {
    public bool canBeActivated;
    public bool isActive;
    public bool isInteractable;
    public bool canBlockRaycast;
    public CanvasGroup cnvsGrp;
    public string temp_CanvasElementTag;

    public enum canvasElement_name
    {
        Main,
        PathLearning,
        Options,
        intBatMain,
        intBatOptions
    };
    public canvasElement_name nameCanvasElement;


    void Start() {
      cnvsGrp = GetComponent<CanvasGroup>();
      On_Visibility("main_canvas");
    }


    public void Off_Visibility(string tag) {
        //Debug.Log(tag);
        GameObject temp_Off = GameObject.FindGameObjectWithTag(tag);
        if (this.gameObject.tag == tag) {
            temp_Off.GetComponent<canvasElement>().cnvsGrp.alpha = 0;
            temp_Off.GetComponent<canvasElement>().isActive = false;
            temp_Off.GetComponent<canvasElement>().isInteractable = false;
            temp_Off.GetComponent<canvasElement>().canBlockRaycast = false;
            temp_Off.GetComponent<canvasElement>().cnvsGrp.blocksRaycasts = false;
            temp_Off.GetComponent<canvasElement>().cnvsGrp.interactable = false;
            temp_Off.GetComponent<canvasElement>().canBeActivated = true;

            EventManager.TriggerEvent("defineVisibilityAndInteraction");
        }
    }

    public void On_Visibility(string tag)  {
        //Debug.Log(tag);
        GameObject temp_On = GameObject.FindGameObjectWithTag(tag);
        if (this.gameObject.tag==tag)
        {
            temp_On.GetComponent<canvasElement>().canBeActivated = false;
            temp_On.GetComponent<canvasElement>().cnvsGrp.alpha = 1;
            temp_On.GetComponent<canvasElement>().isActive = true;
            temp_On.GetComponent<canvasElement>().isInteractable = true;
            temp_On.GetComponent<canvasElement>().canBlockRaycast = true;
            temp_On.GetComponent<canvasElement>().cnvsGrp.blocksRaycasts = true;
            temp_On.GetComponent<canvasElement>().cnvsGrp.interactable = true;

            EventManager.TriggerEvent("defineVisibilityAndInteraction");
        }
    }


    public void defineCanvasElement(string canvasTag) {
        string temp_CanvasElementTag = canvasTag;

        switch (temp_CanvasElementTag) {
            case "main_canvas":
                nameCanvasElement = canvasElement_name.Main;
                break;

            case "options_canvas":
                nameCanvasElement = canvasElement_name.Options;
                break;         

            case "trainingPath_canvas":
                nameCanvasElement = canvasElement_name.PathLearning;
                break;
        }
    }



    #region ON / OFF for canvas panels UI

    public void activateMain(string tag) {
        On_Visibility(tag);
    }

    public void deactivateMain(string tag){
        Off_Visibility(tag);
    }

    public void activateOptions(string tag){
        On_Visibility(tag);

    }

    public void deactivateOptions(string tag){
        Off_Visibility(tag);
    }

    public void activateTrainingpath(string tag) {
        On_Visibility(tag);
    }
    public void deactivateTrainingPath(string tag)
    {
        Off_Visibility(tag);
    }

    public void activeUIInterativeBattery()
    {
        SceneManager.LoadScene("ProductoBateria");
        Debug.Log("Estoy dando click");
    }

    public void deactiveUIInterativeBattery()
    {
        Off_Visibility(tag);
    }

    public void activeUIInterativeBatteryOptions(string tag)
    {
        On_Visibility(tag);
    }

    public void deactiveUIInterativeBatteryOptions(string tag)
    {
        Off_Visibility(tag);
    }

    #endregion

    public void destroyVideoElementsOnBack() {

        GameObject[] Elements = GameObject.FindGameObjectsWithTag("trainingPath_videoElement");
        foreach (GameObject videoelement in Elements) {
            GameObject.Destroy(videoelement);
        }
           
    }

    private void Update()
    {
        defineCanvasElement(this.tag);
    }

}
