using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class faderTrigger : MonoBehaviour {

    public LoadingOverlay fader;
	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
        fader.FadeOut();
    }
}
