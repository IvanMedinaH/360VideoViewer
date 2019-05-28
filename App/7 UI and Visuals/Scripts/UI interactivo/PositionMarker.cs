using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionMarker : MonoBehaviour {

    public Transform player;    
    public Transform positionToCompare;
    public bool canDesactivate;
    CanvasGroup canvas;



	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        canvas = GetComponent<CanvasGroup>();
	}
	
    public void isInPosition()
    {
        if (player.transform.position == positionToCompare.position)
        {
            canDesactivate = true;
            DeactivateMarker();
        }
        if (player.transform.position != positionToCompare.position)
        {         
            canDesactivate = false;
            ActivateMarker();
        }
    }

    public void DeactivateMarker() {
        //this.gameObject.SetActive(false);
        canvas.alpha = 0;
        canvas.interactable = false;
        canvas.blocksRaycasts = false;
    }
    public void ActivateMarker()
    {
        //this.gameObject.SetActive(true);
        canvas.alpha = 1;
        canvas.interactable = true;
        canvas.blocksRaycasts = true;
    }

    

    // Update is called once per frame
    void Update () {
        isInPosition();
        
	}
}
