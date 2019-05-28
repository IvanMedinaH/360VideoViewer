using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.XR;

public class UI_OptionsManager : MonoBehaviour {

    public Light generalLight;
    [Range(0.0f, 100.0f)]
    public float lighIntensity_value;
    public AudioSource generalAudio;
    [Range(0.0f, 10.0f)]
    public float generalVolume_value;
    public Slider slider_Volume;
    public Slider slider_Light;
    public VideoPlayer player;



    public void raise_Volume() {
      //player.audioOutputMode = UnityEngine.Video.VideoAudioOutputMode.AudioSource;
      // player.SetTargetAudioSource(10, generalAudio);
    
        generalVolume_value = slider_Volume.value;
    }

    public void raise_Lighting() {
       lighIntensity_value= slider_Light.value;
        generalLight.range = slider_Light.value;
    }
	// Use this for initialization
	void Start () {
       // XRSettings.eyeTextureResolutionScale = 2;
    }
	

	// Update is called once per frame
	void Update () {
        raise_Lighting();
        raise_Volume();
    }

    public void Volume()
    {
        if (player.audioOutputMode == VideoAudioOutputMode.Direct)
        {
            player.SetDirectAudioVolume(0, slider_Volume.value);
        }
        else if (player.audioOutputMode == VideoAudioOutputMode.AudioSource)
        {
            player.GetComponent<AudioSource>().volume = slider_Volume.value;
        }
        else {
            player.GetComponent<AudioSource>().volume = slider_Volume.value;
        }
            
    }

}
