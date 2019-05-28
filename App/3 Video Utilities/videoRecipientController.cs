using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class videoRecipientController : MonoBehaviour {
    #region VARIABLES
    [Header("Manager to control the load of the clip owned by this recipient")]
    [Space(10)]
    [Header("Atributes")]
    public float timer;
    public VideoPlayer player;
    public bool isPlaying = false;
    public bool isStopped = false;
    public bool isLoaded = false;
    public bool isdone = false;
    public bool canPlay = false;
    bool isFirstRun = false;

    [Space(10)]
    [Header("Clip Assigned to this recipient : ")]  
    public VideoClip clip;

    [Space(10)]
    [Header("On audio/video prepared: ")]
    public bool isAudioPrepared = false;
    public bool isVideoPrepared = false;

    [Space(10)]    
    public Video video;

    [Space(10)]
    [Header("Video attributes : ")]
    public RecipientAttributes atribute;
    #endregion


    // Use this for initialization
    void Start() {
        this.player = gameObject.AddComponent<VideoPlayer>();
        atribute = GetComponent<RecipientAttributes>();
        this.video = GetComponent<Video>();
        clip = video.clip;

        player.clip = clip;
        player.playOnAwake = false;
        player.waitForFirstFrame = false;
        player.Stop();
    }



    #region Video Utilities
    IEnumerator playVideo() {      
        isFirstRun = false;
        player.playOnAwake = false;
        player.Pause();

        player.Prepare();
        isVideoPrepared = true;       
        
        //Wait until video is prepared
        while (!player.isPrepared)
        {
            isVideoPrepared = false;
            yield return null;
        }
        //Debug.Log("Done Preparing Video"); /*<--- the video has been finished to prepare*/
        //Play Video
        player.Play();
        //Debug.Log("Playing Video");        /*<--- the video has been finished to prepare*/
        while (player.isPlaying)
        {
            Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)player.time));
            yield return null;
        }
        //Debug.Log("Done Playing Video");  /*<--- the video has been finished to prepare*/
    }

    public void checkIfCanPlayVideo() {
         canPlay=atribute.canPlayPreview;
    }

    public void PlayStop()
    {       
            if (!isStopped )
            {
               player.Stop();
            }
            else if (isStopped)
            {
                player.Play();               
            }
            else
            {
                StartCoroutine(playVideo());
            }        
    }
    #endregion 


    #region loop Logic
    void FixedUpdate () {

        checkIfCanPlayVideo();
        /*if (Input.GetKeyDown(KeyCode.Return)) {
            PlayStop();
        }*/
        if (canPlay){
            video.calculateDuration(player);
            PlayStop();            
        }
       else /*if (atribute.isHighlighted == true && atribute.canPlayPreview == true)*/
        {
            isStopped = true;
            isPlaying = false;
            player.Stop();
        }
    }
    #endregion
}
