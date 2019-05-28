using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class sceneController : MonoBehaviour {

    public float timeScene;
    public string nameScene;
    public float timeExtra = 0;
    public VideoPlayer videoPlayer;
   

    // Use this for initialization
    void Awake () {
     
       timeScene = (float)videoPlayer.clip.length;


        
        Debug.Log("totalCurrent" + videoPlayer.clip.length);
        StartCoroutine(newScene());   
    }

    private void Update()
    {
        
    }

    IEnumerator newScene() {
        yield return new WaitForSeconds(timeScene + timeExtra);
        SceneManager.LoadScene(nameScene);

    }


   
}
