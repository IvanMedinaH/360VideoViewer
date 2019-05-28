using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class MainBtn_Section : MonoBehaviour {

   // public TextMeshPro title;
    public bool isActive;
    public GameObject Panel;
private void Awake()
    {
    //    title = GetComponentInChildren<TextMeshPro>();
        Panel = GameObject.FindGameObjectWithTag("mainBtnPanelSection");
    }


    // Use this for initialization
  /*  public void deactivate_textMeshPro() {
        isActive = false;
       this.title.gameObject.SetActive(false);

    }
    
    public void Activate_textMeshPro()
    {
        isActive = true;
        title.gameObject.SetActive(true);
    }
    */

    public void deactivateThisPanel() {
        Panel.SetActive(false);
    }


    public void ActivateThisPanel()
    {
        Panel.SetActive(true);
    }


    void Update()
    {
       /* if (this.title.gameObject.activeInHierarchy)
        {
            isActive = true;
        }
        else {
            isActive = false;
        }*/

    }

}
