using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionManager : MonoBehaviour {

    /* public Transform DefaultPos;
     public Transform interactive_LTH_Optima;
     public Transform interactive_Productos;
     public Transform interactive_Quiz;   
     public Transform [] AvailablePositions = new Transform[3];
     */
    public Transform player;
    Transform temp_LastPosition;
    Transform temp_NewPosition;


    // Use this for initialization
    void Start () {
       /* AvailablePositions[0] = interactive_LTH_Optima;
        AvailablePositions[1] = interactive_Productos;
        AvailablePositions[2] = interactive_Quiz;*/

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void calculateCurrentPosition() {

    }

    public void getNewPosition() {        
    }

    public void changePosition(Transform newPosition) {
        player.position = newPosition.position;
    }

}
