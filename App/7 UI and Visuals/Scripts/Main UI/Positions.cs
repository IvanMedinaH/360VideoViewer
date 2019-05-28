using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Positions : MonoBehaviour {

    
    Transform [] temp_childrens;
    GameObject temp;    

    [Header("available positions")]
    public Vector3[] positions;
    
    
    public void initializeAllPositions() {
        temp = GameObject.FindGameObjectWithTag("Parent");
        temp_childrens = Array.FindAll(temp.GetComponentsInChildren<Transform>(), child => child != this.transform);
    }


    public void CreateChildrenArray_Components(){
        int i = 0;
        positions = new Vector3 [temp_childrens.Skip(1).Count()];

        foreach (Transform child in temp_childrens.Skip(1)){
            positions[i] = child.transform.position;
            positions[i].y = 1.0f;
            i++;
        }       
    }
  

    void Update () {
        initializeAllPositions();
        CreateChildrenArray_Components();

    }
}
