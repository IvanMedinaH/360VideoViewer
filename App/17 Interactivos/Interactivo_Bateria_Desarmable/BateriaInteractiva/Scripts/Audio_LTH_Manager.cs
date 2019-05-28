using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class Audio_LTH_Manager : MonoBehaviour {


    
    [Header("Blur effect")]
    public GameObject blurredObject;


    [Header("-----------------------------")]
    [Header("Panel Inicio")]
    public CanvasGroup MainPanel;
    [Header("Panel interactivo")]
    public CanvasGroup InteractivoPanel;
    [Header("-----------------------------")]

    [Header("Sonidos")]
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    [Header("-----------------------------")]



    bool canPlayAudio = false;




    [Header("UI")]
    public Button ButtonIniciar;

   

    // Use this for initialization
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start() {
       // XRSettings.eyeTextureResolutionScale = 2;
    }

    

    // Update is called once per frame
    void Update() {
    }


    IEnumerator habilitarMenuOpciones(CanvasGroup cnvs) {
        yield return new WaitForSeconds(2.5f);
        ActivatePanel(cnvs);
    }


    public void waitToActivatePanel(CanvasGroup cnvs) {
        StartCoroutine("habilitarMenuOpciones", cnvs);
    }



    public  void ActivatePanel(CanvasGroup cnvs) {
        cnvs.alpha = 1;
        cnvs.blocksRaycasts = true;
        cnvs.interactable = true;
    }
    public void DeactivatePanel(CanvasGroup cnvs)
    {
        cnvs.alpha = 0;
        cnvs.blocksRaycasts = false;
        cnvs.interactable = false;
    }


          
    public void backToMainLobby() {
        SceneManager.LoadScene("MainLobbyRoom");
    }


    public void PlayAudio(int posArray)
    {
        if (InteractivoPanel.alpha ==1) {
            canPlayAudio = true;
        }

        if (canPlayAudio)
        {
            audioSource.clip = audioClips[posArray];
            audioSource.Play();

            Debug.Log("Si entra al audio");
        }


    }


}
