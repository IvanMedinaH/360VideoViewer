using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(JSON_SectionConfig))]
public class Section : MonoBehaviour {

    [Header("Training path information")]
    [SerializeField]
    private string numOfElementsInSection;
    [SerializeField]
    private bool isCompleted;
    [SerializeField]
    private float percentageCompleted;
    [Header("------------------------------")]
    [Header("Name of the section")]
    [SerializeField]
    private string sectionname;

    [Header("Text name container: ")]
    public Text NameTextUI;
    public TextMeshPro sectionName;

    [Header("Path to the training videos: ")]
    public string sectionPath;

    [Header("------------------------------")]
    [Header("Number of elements in path: ")]
    public int numbOfElementsinPath;


    [Header("Flag verifying elements in trainingPath")]
    public bool canCheckForElementsInSection = false;
    //---------------------------------------------------------------------------
    public string[] pathlistTemp;
    public int pathElement;
    

  

    void Awake()
    {    
      this.NameTextUI = GetComponentInChildren<Text>();
      this.sectionName = GetComponentInChildren<TextMeshPro>();

    }

     void Start()
    {
        pathlistTemp = GetComponent<JSON_SectionConfig>().Listpaths;
    }

    #region Properties encapsulated
    public string SectionName{
       get
        {
            return sectionname;
        }

        set
        {
            sectionname = value;
        }
    }
    public bool IsCompleted
    {
        get
        {
            return isCompleted;
        }

        set
        {
            isCompleted = value;
        }
    }
    public string NumOfElementsInSection
    {
        get
        {
            return numOfElementsInSection;
        }

        set
        {
            numOfElementsInSection = value;
        }
    }
    public float PercentageCompleted
    {
        get
        {
            return percentageCompleted;
        }

        set
        {
            percentageCompleted = value;
        }
    }
    #endregion


    #region Section utilities

    public void calculatePercentage(){
    }
    
    public void calculateIfTrainingIsCompleted() {
    }


    public void retriveSectionInformation(){
        if (sectionname != string.Empty)
        {
            canCheckForElementsInSection = true;
        }
        else {
            canCheckForElementsInSection = false;

        }
    }

    public string retrievePathInformation() {
        int i = 0;
        foreach (string name in pathlistTemp) {

            if (name.Contains(sectionname))
            {
                this.sectionPath = name;
                return sectionPath;
            }
        }
        return sectionPath;
    }

    public int loadElementsFromSectionPath()
    {
        Object[] tempObj = Resources.LoadAll(sectionPath);
        numbOfElementsinPath = tempObj.Length;
        
        //Number of elements in a path
        //Debug.Log(numbOfElementsinPath);
        return numbOfElementsinPath;
    }
    #endregion





    #region loopLogic
    void Update()
    {
      retriveSectionInformation();
      sectionPath = retrievePathInformation();
       
    }
    #endregion




}

