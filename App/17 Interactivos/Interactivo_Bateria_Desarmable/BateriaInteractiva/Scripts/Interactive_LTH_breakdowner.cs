using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive_LTH_breakdowner : MonoBehaviour {


    public Animator anim;
    public bool canBreakDown;
    public GameObject battery;
    public GameObject HidingObject;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void setCanBreakDownTrue() {
        canBreakDown = true;
    }

    public void setCanBreakDownFalse()
    {
        canBreakDown = false;
    }




    public void breakDownTheBattery(bool canBreakDown)
    {
        anim.SetBool("canBreakDown", canBreakDown);
        anim.SetBool("canCloseDown", false);
        
    }
    /*
    HidingObject.SetActive(true);
    HidingObject.SetActive(false);
    */

    public void closeDownTheBattery() {
        anim.SetBool("canCloseDown", true);
        anim.SetBool("canBreakDown", false);
    }


    void Update()
    {
        if (canBreakDown == true)
        {
            breakDownTheBattery(canBreakDown);
        }

        if (canBreakDown == false) {
            closeDownTheBattery();
        }
    }





}
