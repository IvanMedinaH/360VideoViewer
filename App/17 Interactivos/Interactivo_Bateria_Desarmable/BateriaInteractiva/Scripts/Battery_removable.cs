using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery_removable : MonoBehaviour {

    public bool canCloseDown;
    public bool canBreakDown;
    public Animator anim;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void BreakdownBattery(bool canBreakDown) {
        anim.SetBool("canBreakDown", canBreakDown);
        anim.SetBool("canCloseDown", false);
    }

    public void CloseDownBattery(bool canCloseDown) {
        anim.SetBool("canCloseDown", canCloseDown);
        anim.SetBool("canBreakDown", false);
    }

    public void Update()
    {
        if (canCloseDown == true) {
            CloseDownBattery(true);
            BreakdownBattery(false);
        }
        if (canBreakDown==true) {
            BreakdownBattery(true);
           CloseDownBattery(false);
        }
    }




}
