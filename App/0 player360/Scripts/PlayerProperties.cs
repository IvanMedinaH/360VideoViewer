using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : MonoBehaviour {

    [Header("Flags")]
    public bool CanPlayVideo = false;
    public bool canBeSeen;    
    [Header("Scales")]
    public Vector3 finalScale;
    public Vector3 lastScale;
    [Header("Position")]
    public Vector3 Selectedposition;
    [Header("Speed of transition")]
    [Range(0.0f,2.0f)]
    public float lerpSpeed;
    [Header("VR Sphere scalable object")]
    public GameObject vrSphere;
    // Use this for initialization

    void Awake() {
        canBeSeen = true;
        lastScale = this.transform.localScale;        
    }

    public void canLerpScale(float lerpspeed) {
        if (CanPlayVideo){
            vrSphere.transform.localScale = Vector3.Lerp(vrSphere.transform.localScale, finalScale, lerpSpeed * Time.deltaTime);
        }
        if (CanPlayVideo==false) {
            vrSphere.transform.localScale = Vector3.Lerp(vrSphere.transform.localScale, lastScale, lerpSpeed * Time.deltaTime * 2.0f);
        }       
    }

    public void VisibilityCheck() {
        if (canBeSeen)
        {
            vrSphere.SetActive(canBeSeen);
            CanPlayVideo = true;
        }
        else {
            canBeSeen = false;
            vrSphere.SetActive(canBeSeen);
            CanPlayVideo =false;
        }            
    }

    
    // Update is called once per frame
    void Update () {
        VisibilityCheck();
        canLerpScale(lerpSpeed);
        

    }
}
