using System;
using System.Collections.Generic;
using UnityEngine;

class Sphere : MonoBehaviour
{
    public GameObject SphereContainer;
    public bool isVisible;
    public bool isPlaying;
    public Vector3 Finalsize;
    public Vector3 temp_LastScale;
    public float lerpspeed;
    public Vector3 pivotPoint;

    private void Update()
    {
        SphereContainer = GameObject.FindGameObjectWithTag("parent_videoContainer");
    }

}

