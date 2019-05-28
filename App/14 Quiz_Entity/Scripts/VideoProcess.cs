using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoProcess : MonoBehaviour {

    [Header("The video player")]
    public VideoPlayer videoPlayer;
    public loader namePlayer;
    [Header("The main canvas container")]
    public UI_SectionHandler sectionName;
    public OnClickManager numVideos;

    GameObject idVideo;
    [Header("-------------------------")]
    [Header("Check for video playing")]
    public bool OnVideoPlayer = false;
    public float timer;
    [Header("-------------------------")]
    [Header("Lists of training path elements")]
    public TrainingPath myNewLists;
    public static bool[] arraybool;

    // Use this for initialization
    void Start () {
        videoPlayer = GameObject.FindObjectOfType<VideoPlayer>();
        arraybool = new bool[5];
    }
	
	// Update is called once per frame
	void Update () {

       // Debug.Log("Cuanto dura el video" + videoPlayer.time);

        if (OnVideoPlayer)
        {
            
            timer += 1 * Time.deltaTime;
           // Debug.Log("Este es el tiempo final del video " + videoPlayer.clip.length);
            if (timer >= (float)videoPlayer.clip.length) {
                
                isCompleted(sectionName.sectionToLoad);
            }
        }
        else {
            OnVideoPlayer = false;
            timer = 0;
        }


       if (Input.GetKeyDown(KeyCode.S)) {
            TurnOnQuizSection(arraybool);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            SceneManager.LoadScene("MainLobbyRoom");
        }

    }

    public void isCompleted(string nameseccion)
    {
        for (int i = 0; i < myNewLists.sectionNames.Length; i++) {

            if (nameseccion == myNewLists.sectionNames[i]) {
                addVideoList(myNewLists.strSeccionList[i].ListaNameSections);
                break;
            }
           
        }

    }

    public void videoOn() {
        OnVideoPlayer = true;
    }



    public void addVideoList(List<string>listas) {
        idVideo = GameObject.Find(namePlayer.videoName);
        StateVideo estadovideo = idVideo.GetComponent<StateVideo>();
        estadovideo.visto();
        int conttemp = 0;
        foreach (string listab in listas)
        {
            if (listab == namePlayer.videoName)
            {
                conttemp++;
                Debug.Log("Cuantas veces pasa esto" + conttemp);
            }
        }

        if (conttemp < 1)
        {

            listas.Add(namePlayer.videoName);
        }

        Debug.Log("Fue visto por completo el video? " + estadovideo.visto());
        OnVideoPlayer = false;

    }

    public void TurnOnQuizSection(bool[] array) {
        for(int i = 0; i < myNewLists.sectionNames.Length; i++) {
            QuizActive(myNewLists.strSeccionList[i].ListaNameSections);
            array[i] = QuizActive(myNewLists.strSeccionList[i].ListaNameSections);
            Debug.Log("Quien esta activado " + i + " : " + QuizActive(myNewLists.strSeccionList[i].ListaNameSections));
        }
    }

    public bool QuizActive(List<string> numList)
    {
        bool isItViewed = false;
        if (numList.Count >= numVideos.amountOfVideo_buttons && numList.Count != 0) {
            isItViewed = true;
        }
        return isItViewed;
    }
}
