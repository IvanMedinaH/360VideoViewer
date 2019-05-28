using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive_LTH_panels : MonoBehaviour {

    public Animator anim;
    public bool canOpenPanel;
    public bool canClosePanel;


    private void Start()
    {
     anim = GetComponent<Animator>();
    }


    public void openInteractiveAudioPanel(bool canOpenPanel) {
        anim.SetBool("canOpenPanel",canOpenPanel);

    }



    public void CloseInteractiveAudioPanel(bool canClosePanel)
    {
        anim.SetBool("canClosePanel", canClosePanel);

    }




}
