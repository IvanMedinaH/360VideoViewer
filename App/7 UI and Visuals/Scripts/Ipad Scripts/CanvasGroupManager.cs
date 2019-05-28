using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGroupManager : MonoBehaviour {

    public CanvasGroup canvasElement;



    private void Start()
    {
        canvasElement = GetComponent<CanvasGroup>();
    }


    public void DeActivateCanvas()
    {
        canvasElement.alpha = 0;
        canvasElement.interactable = false;
        canvasElement.blocksRaycasts = false;
    }


    public void ActivateCanvas()
    {
        canvasElement.alpha = 1;
        canvasElement.interactable = true;
        canvasElement.blocksRaycasts = true;
    }



}
