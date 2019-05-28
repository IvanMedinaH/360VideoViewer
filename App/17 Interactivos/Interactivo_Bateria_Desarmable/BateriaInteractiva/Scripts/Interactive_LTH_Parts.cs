using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive_LTH_Parts : MonoBehaviour {

    public Animator anim;
    public bool canRotate;
    public GameObject part;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }


    public void setCanRotateTrue()
    {
        canRotate = true;
    }

    public void setCanRotateFalse()
    {
        canRotate = false;
    }



    public void rotateThisPart(bool canRotate) {
        anim.SetBool("canRotate", canRotate);
    }


    public void StopRotationOfParts()
    {
        anim.SetBool("canRotate", false);
    }

    void Update()
    {
        if (canRotate)
        {
            rotateThisPart(canRotate);
        }
        else {
            StopRotationOfParts();
        }
        
    }


}
