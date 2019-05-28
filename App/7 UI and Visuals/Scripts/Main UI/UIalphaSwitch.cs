using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIalphaSwitch : MonoBehaviour {

    public CanvasGroup cnvsGrp;


	// Use this for initialization
	void Start () {
        cnvsGrp = GetComponent<CanvasGroup>();
    }


    public void switchOffPanel() {
        this.cnvsGrp.alpha = 0;
        this.cnvsGrp.interactable = false;
        this.cnvsGrp.blocksRaycasts = false;
    }

    public void switchOnPanel() {
        this.cnvsGrp.alpha = 1;
        this.cnvsGrp.interactable = true;
        this.cnvsGrp.blocksRaycasts = true;
    }


    


}
