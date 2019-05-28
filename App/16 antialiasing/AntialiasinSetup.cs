using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class AntialiasinSetup : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        XRSettings.eyeTextureResolutionScale = 1.5f;
        OVRManager.tiledMultiResLevel = OVRManager.TiledMultiResLevel.LMSHigh;
        OVRManager.display.displayFrequency = 72.0f;
    }

    // Update is called once per frame
    void Update () {
       
    }
}
