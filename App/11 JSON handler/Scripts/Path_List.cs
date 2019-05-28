using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Path_List{

    //remember to ALWAYS call this variable the same as the OBJECT to serialize, the class base
    [Header("Paths")]
    public List<Paths> Paths = new List<Paths>();
    
}
