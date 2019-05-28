using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingPath : MonoBehaviour {

    TrainingEntity strSeccion_list = new TrainingEntity();
    [Header("-------------------------")]
    [Header("Array of lists for store video names")]
    public TrainingEntity[] strSeccionList;
    [Header("-------------------------")]
    [Header("Array of section names")]
    public string[] sectionNames;

    [Header("-------------------------")]    
    public JSON_Handler numSections;

    /*
    public void Start()
    {
        strSeccionList = new TrainingEntity[numSections.numbOfSectionButtons];
        sectionNames = new string[numSections.numbOfSectionButtons];
        int tempI = 0;
        foreach (string sectionString in numSections.sectionNames) {
            Debug.Log(sectionNames[tempI] = sectionString);
            tempI++;
        }
    }*/

    public void Start()
    {
        StartCoroutine(startList());
    }

    IEnumerator startList()
    {
        yield return new WaitForSeconds(2);
        strSeccionList = new TrainingEntity[numSections.numbOfSectionButtons];
        sectionNames = new string[numSections.numbOfSectionButtons];
        int tempI = 0;
        foreach (string sectionString in numSections.sectionNames)
        {
            //print Section Names
            //Debug.Log(sectionNames[tempI] = sectionString);
            tempI++;
        }
    }


}
