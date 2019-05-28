using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipientAttributes : MonoBehaviour {
    [SerializeField]
    public bool isHighlighted=false;
    public bool canPlayPreview = false;
    public bool canLerpPosAndScale=false;
   

    public int temp_position;
    public int positionAssingned;
    public Material highlighted;
    public Material notHighlighted;
    public Material Selected;

 
    public Input_RecipientPosController controller;


    #region initialize Stuff
    private void Start()
    {
        controller = GameObject.FindGameObjectWithTag("AppManager").GetComponentInParent<Input_RecipientPosController>();        
    }
    #endregion
    /*******************************************************************************************/
    #region change material
    public void ChangeMaterial(int selection) {
        if (isHighlighted && positionAssingned == selection) {
            GetComponent<Renderer>().material = highlighted;
        } else {
            GetComponent<Renderer>().material = notHighlighted;
            
        }
        if (isHighlighted && positionAssingned == selection && controller.isMaterialSelectedOn == true)
        {
            canPlayPreview = true;
            GetComponent<Renderer>().material = Selected;
        }else {
            canPlayPreview = false;
        }
    }
    #endregion
    /*******************************************************************************************/
    #region highlight the current recipient if selected
    public void setSelected( int selection){
        if (positionAssingned != selection){
            isHighlighted = false;
        }else{
            isHighlighted = true;
        }
    }
    #endregion

    /*******************************************************************************************/
  
    #region Get the position assigned
    public int GetPositionAssigned()
    {
        return positionAssingned;
    }
    #endregion

    #region loop attributes
    void Update()
    {

        temp_position = controller.currentPosition;
        canLerpPosAndScale = controller.canLerpPosition;

        ChangeMaterial(temp_position);
        setSelected(temp_position);
         
    }
    #endregion



    /*
    public void defaultSelected(){
        if (positionAssingned == 0) {
            Debug.Log(positionAssingned);
            IsSelected = true;
            ChangeMaterial();
        }else{
            IsSelected = false;
            ChangeMaterial();
        }
    }*///Not using


}
