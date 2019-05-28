using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
	
    public Transform CanvasArrow;
    public Transform player;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void facePlayer()
    {
        CanvasArrow.LookAt(player.position /*, Vector3.up*/);
    }

 

}
