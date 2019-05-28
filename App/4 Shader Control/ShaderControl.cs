using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderControl : MonoBehaviour {

    [Header("Amount of material disolved")]
    public float amountDisolve;

    [Header("Disolve speed")]
    public float lerpSpeed;
    float temp_lerp;

    [Header("Is it DisolvedButNotHighlighted")]
    public bool disolvedNotHighlighted = false;

    [Header("Is it Disolved")]
    public bool isDisolved = false;

    [Header("Control flags for Integration / Disintegration")]
    public bool canReintegrate = false;
    public bool canDisintegrate = false;
    
    [Header("Materials :")]
    public Material Disolve;
    public Material selected;
    [Header("Object renderer")]
    public MeshRenderer mat_properties;
    Shader customShader;

    private void Start()
    {
        gameObject.GetComponent<Renderer>().sharedMaterial = selected;
    }


    /*OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO*/

    public void changeMaterialtoDisolved() {
        gameObject.GetComponent<Renderer>().sharedMaterial = Disolve;        
     }

    public void changeMaterialToSelected() {
        gameObject.GetComponent<Renderer>().sharedMaterial = selected;
    }

 
    public void disolveMaterial(float amount) {
        gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_disolveAmount", amount);        
    }
    
    
    public void disintegration_Transition() {
        disintegration();
        disolveMaterial(amountDisolve);
    }


    public void Integration_Transition() {
        reintegration();
    }

 /*OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO*/

    public void checkIntegrity() {

        if (gameObject.GetComponent<Renderer>().sharedMaterial == selected)
        {
            isDisolved = false;
            canDisintegrate = true;
            canReintegrate = false;
        }

        
        if (gameObject.GetComponent<Renderer>().sharedMaterial == Disolve) {
            if (gameObject.GetComponent<Renderer>().sharedMaterial.GetFloat("_disolveAmount") == 10)
            {
                isDisolved = true;
                canDisintegrate = false;
                canReintegrate = true;
            }

            if (gameObject.GetComponent<Renderer>().sharedMaterial.GetFloat("_disolveAmount") == 0)
            {
                isDisolved = false;
                canDisintegrate = true;
                canReintegrate = false;
            }
        }
        
        
    }

    public void Reset()
    {
        lerpSpeed = 0;
        amountDisolve = 0;
    }

 /*OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO*/
 
    void Update () {        

        if (isDisolved == false && canDisintegrate == true)
        {
            disintegration_Transition();
            disolveMaterial(amountDisolve);
        }
        
        if (isDisolved == true && canReintegrate==true) {
            Integration_Transition();          
            disolveMaterial(amountDisolve);
            
        }


    }

    /*OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO*/
    /// <summary>
    /// Lerp around the disolveAmount value of the shader to create the reintegration effect
    /// </summary>
    public void reintegration()
    {
        lerpSpeed += 0.5f * Time.deltaTime;
        amountDisolve = Mathf.Lerp(10.0f, 0.0f, lerpSpeed);
        if (amountDisolve == 0.0f)
        {           
            if (isDisolved==false) {
                
            }
            canReintegrate = false;
            isDisolved = false;
            amountDisolve = 0;
            lerpSpeed = 0;
            changeMaterialToSelected();
        }
        else
        {
            return;
            isDisolved = true;            
        }
    }

    /// <summary>
    /// Lerp around the disolveAmount value of the shader to create the disintegration effect
    /// </summary>
    public void disintegration()
    {
        changeMaterialtoDisolved();
        lerpSpeed += 0.5f * Time.deltaTime;
        amountDisolve = Mathf.Lerp(0.0f, 10.0f, lerpSpeed);
        if (amountDisolve == 10.0f)
        {
            isDisolved = true;
            canDisintegrate = false;
            return;
        }
        else
        {
            disolveMaterial(amountDisolve);
            isDisolved = false;
            canReintegrate = false;
        }
    }

}
