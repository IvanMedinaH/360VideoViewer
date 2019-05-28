using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;



public class RecipientArrayManager : MonoBehaviour {

    public Transform[] children;
    public GameObject Parent;
    public GameObject [] attribute_Obj;
    Input_RecipientPosController controller;




	// Use this for initialization
	void Awake () {
        controller = GetComponent<Input_RecipientPosController>();
	}

    public void initializeArrayComponent() {        
        children = new Transform[Parent.transform.childCount];
        //children = Parent.transform.gameObject.GetComponentsInChildren<Transform>();        
        children = Array.FindAll(Parent.transform.gameObject.GetComponentsInChildren<Transform>(), child => child != this.transform);
    }

    public void CreateChildrenArray_Components() {
        int i=0;
        attribute_Obj = new GameObject[children.Skip(1).Count()];
        foreach (Transform child in children.Skip(1)) {            
            attribute_Obj[i] = child.gameObject;
            i++;
        }
    }

    /*
    public GameObject ChangeAttribute(int pos) {
        GameObject tempAttribute = null;
        foreach (GameObject obj in attribute_Obj)
        {
             if(obj.name.Substring(5) == pos.ToString() ){
             //if (pos == obj.GetComponent<CubeAttributes>().positionAssingned) 
                tempAttribute = obj;
                return tempAttribute;
            } 
        }
            return tempAttribute;
    }*/




    void Update () {
        initializeArrayComponent();
        CreateChildrenArray_Components();        
    }
}

