using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_RecipientPosController : MonoBehaviour {

    public int currentPosition = 0;
    public CreateRecipientArray array;
    public bool canLerpPosition=false;
    public bool isMaterialSelectedOn = false;
    int val = 0;


    void Start(){
        array = GetComponent<CreateRecipientArray>();
    }

    #region INPUT LEFT or RIGHT to change POSITION SELECTION
    void Update() {
       
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            MoveNextElement(); 
            EventManager.TriggerEvent("goNextPosition");
            EventManager.TriggerEvent("changeUI_Position");
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            MovePreviousElement();
            EventManager.TriggerEvent("goPreviusPosition");
            EventManager.TriggerEvent("changeUI_Position");
        }

        if (currentPosition < 0) {
            currentPosition = 0;
        }
        if (currentPosition > array.cubes.Length) {
            currentPosition = array.cubes.Length;
            Debug.Log(array.cubes.Length);
        }        
        #region Enter Select video preview
        if (Input.GetKeyDown(KeyCode.Return)){
           // Debug.Log("object can lerp position : " + canLerpPosition);
            BinaryEvaluation_selector();
        }
        #endregion        
    }
    #endregion

    #region Mod % 2
    public void BinaryEvaluation_selector(){
        int temp=1;
            temp = val % 2;
            val++;
        if (temp == 1)
        {
            canLerpPosition = true;
            isMaterialSelectedOn = true;
            EventManager.TriggerEvent("disintegrate");
            //Debug.Log("Can lerp Position: " + canLerpPosition);
            //return canLerpoPosition;
        }
        if (temp == 0)
        {
            canLerpPosition = false;
            isMaterialSelectedOn = false;
            EventManager.TriggerEvent("Integrate");
            //Debug.Log("Can lerp Position: " + canLerpPosition);
            //return canLerpoPosition;
        }
    }
    #endregion


    #region move next element
    public void MoveNextElement(){
        //this.transform.position = new Vector3();
        currentPosition = currentPosition  + 1 ;     
    }
    #endregion
    #region move previous element
    public void MovePreviousElement (){
        
        currentPosition = currentPosition - 1;
    }
    #endregion

}
