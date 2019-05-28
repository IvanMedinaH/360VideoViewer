using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreenController : MonoBehaviour {

    public CanvasGroup MainCanvasGrp;
    public CanvasGroup QuizPanelGrp;

    public void fadeOut() {
        MainCanvasGrp.alpha = 0;
        MainCanvasGrp.interactable = false;
        MainCanvasGrp.blocksRaycasts = false;

        QuizPanelGrp.alpha = 1;
        QuizPanelGrp.interactable = true;
        QuizPanelGrp.blocksRaycasts = true;
    }

    public void StartGame() {
        fadeOut();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
