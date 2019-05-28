using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Transform myPosition;
    public bool canChangePosition;


	// Use this for initialization
	void Start () {
        myPosition.position = this.transform.position;
             
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
