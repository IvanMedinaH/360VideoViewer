using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class visibilityManager : MonoBehaviour {

    
    public GameObject mainControllerUI;
    public int autoHideControlsTime = 0;
    public bool showPlayerControls = false;
    private float hideScreentime = 0;
    public CanvasGroup canvasGroup;
    public GameObject btnText;

    [Header (" PanelText ")]
    public GameObject panelText;

    
    
    Quaternion controllerOculus;

    private void Awake()
    {
        if (showPlayerControls)
        {
            if (!mainControllerUI.CompareTag("mainUI"))
            {
                hideControllers();
                Debug.Log("Adentro");
            }
        }
    }

    void Update () {



        

        controllerOculus.z = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote).z;
        


       // Debug.Log(controllerOculus);

        if (showPlayerControls) {
            if (autoHideControlsTime > 0)
            {
                if (userInteract()) {
                    hideScreentime = 0;
                    if (mainControllerUI != null)
                        canvasGroup.alpha = 1;
                    canvasGroup.interactable = true;
                    canvasGroup.blocksRaycasts = true;
                    btnText.SetActive(true);
                    panelText.SetActive(true);
                   // btnMenuText.SetActive(true);
                       // mainControllerUI.SetActive(true);
                }
                else
                {
                    hideScreentime += Time.deltaTime;
                    if (hideScreentime >= autoHideControlsTime) {
                        hideScreentime = autoHideControlsTime;
                        hideControllers();
                    }
                }
               
            }
        }
	}

    //Devuelve verdadero si usa uno de los botones de los oculues
    bool userInteract(){
                #if UNITY_EDITOR
                        //Debug.Log("Unity Editor");
                        if (Application.isMobilePlatform)
                        {
                            if (Input.touches.Length >= 1)
                                return true;
                            else
                                return false;
                        }
                        else
                        {
                            if (Input.GetMouseButtonDown(0))
                                return true;
                            return (Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0);
                        }
#endif
#if UNITY_ANDROID
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
                        {
                            return true;
                        }
                        else {

                            return controllerOculus.z <= -0.2f || controllerOculus.z >= 0.2f;
                        }
                #endif
    }

    private void hideControllers() {

        if (mainControllerUI != null) {
            canvasGroup.alpha = 0;
            canvasGroup.interactable =false;
            canvasGroup.blocksRaycasts = false;
            btnText.SetActive(false);
            panelText.SetActive(false);
            // btnMenuText.SetActive(false);
            //  mainControllerUI.SetActive(false);
        }
    }

    public void returnScene(string nameSceneReturn) {
        SceneManager.LoadScene(nameSceneReturn);
    }
}
