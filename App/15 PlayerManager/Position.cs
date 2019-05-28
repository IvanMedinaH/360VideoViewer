using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : MonoBehaviour {
    public Transform CurrentPosition;
    public bool isInPosition;
    public Transform player;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        CurrentPosition.position = this.transform.position;
    }


    public void isPlayerInPosition() {
        if (player.position == CurrentPosition.position)
        {
            isInPosition = true;
        }
        else {
            isInPosition = false;
        }
    }

    private void Update()
    {
        isPlayerInPosition();
    }


}
