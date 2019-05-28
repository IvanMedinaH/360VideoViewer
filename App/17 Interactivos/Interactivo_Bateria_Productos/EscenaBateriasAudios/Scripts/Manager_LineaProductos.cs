using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class Manager_LineaProductos : MonoBehaviour {

    public Animator anim;
    public GameObject mainCabinet;


    [Header("Audios")]
    public AudioClip[] audioClip;
    public AudioSource audiosource;



    [Header("Central Cabinet")]
    public bool canHide;
    public bool canBeRevealed;

    [Header("Interactive floor")]
    public bool CanOpen;




    private void Awake()
    {
       //XRSettings.eyeTextureResolutionScale = 2;
       //XRSettings.eyeTextureResolutionScale = 2;
    }

    public void Start()
    {
        audiosource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        audiosource.clip = audioClip[7];
        audiosource.Play();        
    }


    


    public void ActivateMainCabinet(bool canReveal) {
        anim = GameObject.FindGameObjectWithTag("CentralCabinet").GetComponent<Animator>();
        anim.SetBool("canBeRevealed", canReveal);
        canBeRevealed = canReveal;
        
    }


    public void DeactivateMainCabinet( ) {
        anim = GameObject.FindGameObjectWithTag("CentralCabinet").GetComponent<Animator>();
        canBeRevealed = false;
        canHide = true;
        anim.SetBool("canHide", false);
    }
     
    
    public void ActivateInteractiveFloor(bool CanOpenFloor)
    {
        anim = GameObject.FindGameObjectWithTag("InteractiveFloor").GetComponent<Animator>();
        anim.SetBool("CanOpenFloor", CanOpenFloor);
        CanOpen = CanOpenFloor;

    }

    public void DeActivateInteractiveFloor()
    {
        anim = GameObject.FindGameObjectWithTag("InteractiveFloor").GetComponent<Animator>();
        CanOpen = false;
        anim.SetBool("CanOpenFloor", false);
    }

    private void Update()
    {
        
       
    }





  


    public void audioBattery(int id) {
            //variables de audio
            audiosource.clip = audioClip[id];
            audiosource.Play();

          //  effectOn(id);

    /*       if (idEffectGeneral != tempEffect)

            {
                effectOff(tempEffect);
                tempEffect = id;
            }
           
           // idEffectGeneral = id;
            //tempEffect = idEffectGeneral;
      */     

        }


    /*
    public void effectOn(int id) {

        idEffectGeneral = id;

        effectsGO = GameObject.FindGameObjectsWithTag("Effect_" + id);
        for (int i = 0; i < effectsGO.Length; i++)
        {
            effectsGO[i].GetComponent<LineRenderer>().enabled = true;
            Debug.Log("Si entre");
        }
    }

    public void effectOff(int id) {
        effectsGO = GameObject.FindGameObjectsWithTag("Effect_" + id);
        for (int i = 0; i < effectsGO.Length; i++)
        {
            effectsGO[i].GetComponent<LineRenderer>().enabled = false;
            Debug.Log("Si entre  al off");
        }
    }
    */


    public void cargarEscenaPrincipal() {
        SceneManager.LoadScene("ProductoBateria");
    }

    public int efectoBateria(int id) {
        return id;
    }



}
