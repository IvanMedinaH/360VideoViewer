using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;

[System.Serializable]
public class Video : MonoBehaviour {
    public string Nombre;
    public VideoClip clip;
    public string URL;
    public float Duracion;
    public string Informacion;
    public string Seccion;
    float currentDuration;
    float currentTime;

    public void calculateDuration(VideoPlayer p) {
        //currentDuration = Mathf.RoundToInt(p.frameCount / p.frameRate);
        currentDuration = (float)p.clip.length;
        currentTime = (float)p.time;
        Debug.Log(currentDuration);
        Debug.Log( Math.Round(currentTime, 2));
    }

}
