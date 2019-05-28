using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class selectBattery : MonoBehaviour {

    Animator animator;
    Collider collider;
   public TextMeshProUGUI textDebug;
    string text;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        
    }

    private void Update()
    {
      
    }

    public void activeCollider(GameObject go) {
        collider = go.GetComponent<Collider>();
        collider.enabled = true;
    }

    public void desactiveCollider(GameObject go) {
        collider = go.GetComponent<Collider>();
        collider.enabled = false;
    }

    public void OnRotateBattery()
    {
        animator.SetBool("ActiveRotate",true);
        text = "Entre entro a rotacion";
        Debug.Log("Hi");
        //Rotar parte de la bateria
    }
    public void OffRotateBattery(){
        text = "Sali de rotacion";
        animator.SetBool("ActiveRotate",false);
    }

}
