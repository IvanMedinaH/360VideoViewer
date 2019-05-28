using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpingActions : MonoBehaviour {


    [Header("final flag for lerping")]
    public bool canDoLerping = false;
    [Header("Speed of the lerping action")]
    public float lerpSpeed;
    [Space(10)]
    [Header("Last position and scale")]
    public Vector3 last_PositionKnown;
    public Vector3 last_ScaleKnown;
    [Space(10)]
    [Header("New Position and Scale")]
    public Vector3 new_scale;
    public Vector3 new_Position;
    [Space(10)]
    [Header("Temporary Position and Scale")]
    public Vector3 temp_scale;
    public Vector3 temp_Position;


    Input_RecipientPosController controller;
    RecipientAttributes atribute;
    private void Start()
    {
        controller = GameObject.FindGameObjectWithTag("AppManager").GetComponentInParent<Input_RecipientPosController>();
        atribute = GetComponent<RecipientAttributes>();
        
        new_Position = new Vector3(0, 2.0f, 0);
        new_scale = new Vector3(80,80,80);
        storeLastPosition();
        storeLastScale();
    }

    #region Lerping Actions

    public void LerpPositionAndScale_grow() {
        storeLastPosition();
        storeLastScale();
        /*POSITION*/
        this.gameObject.transform.position = Vector3.Lerp(this.transform.position,new_Position,lerpSpeed * Time.deltaTime);
        /*SCALE*/
        this.gameObject.transform.localScale = Vector3.Lerp(this.transform.localScale, new_scale, lerpSpeed * Time.deltaTime);
    }

    public void Grow()
    {
        storeLastPosition();
        storeLastScale();
        /*POSITION*/
        this.gameObject.transform.position = Vector3.Lerp(last_PositionKnown, new_Position, lerpSpeed * Time.deltaTime);
        /*SCALE*/
        this.gameObject.transform.localScale = Vector3.Lerp(last_ScaleKnown, new_scale, lerpSpeed * Time.deltaTime);
    }

    public void decrease() {
        /*POSITION*/
        this.gameObject.transform.position = Vector3.Lerp(this.transform.position, temp_Position, lerpSpeed * Time.deltaTime);
        /*SCALE*/
        this.gameObject.transform.localScale = Vector3.Lerp(this.transform.localScale,temp_scale, lerpSpeed * Time.deltaTime);
    }

    public void preserveScaleAndPosition() {
        this.gameObject.transform.position = last_PositionKnown;
        this.gameObject.transform.localScale = last_ScaleKnown; 
    }

    #endregion

    /*******************************************************************************************/    
    #region Store values
    public void storeLastPosition(){
        last_PositionKnown = this.transform.position;
        temp_Position = last_PositionKnown;
    }
    public void storeLastScale(){
        last_ScaleKnown = this.transform.localScale;
        temp_scale = last_ScaleKnown;
    }
    #endregion

    /*******************************************************************************************/
    #region loop lerping selection
    void FixedUpdate()
    {
        preserveScaleAndPosition();
        canDoLerping = controller.canLerpPosition;

        if(atribute.isHighlighted && canDoLerping){
            LerpPositionAndScale_grow();
        }     
    }
    #endregion


}
