using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipientAnimation : MonoBehaviour {

    public Vector3 _newPosition;
    public bool canactivateMoveMent = false;
    public float waitTime;
    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        StartCoroutine(waitTillCanMove(waitTime));
    }

    public IEnumerator waitTillCanMove(float waitime) {
        yield return new WaitForSeconds(waitTime);
        canactivateMoveMent = true;
        movement();
    }

    public void movement() {
        if (canactivateMoveMent) { 
        _newPosition = transform.position;
       _newPosition.y += Mathf.Sin(Time.time) * Time.deltaTime * Mathf.PingPong(Time.time, 1);
        transform.position = _newPosition;
        }
    }
}
