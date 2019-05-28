using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarObjeto : MonoBehaviour {
    public GameObject MainCanvas_VideoPlayerControl;



    public void SetActivePlayercontrol() {
        MainCanvas_VideoPlayerControl.SetActive(true);
    }


    public void SetInactivePlayercontrol() {
        MainCanvas_VideoPlayerControl.SetActive(false);
    }



    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
