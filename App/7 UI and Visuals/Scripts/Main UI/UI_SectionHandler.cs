using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SectionHandler : MonoBehaviour {

    [Header("the path to load the videos for training")]
    public string PathReceived;

    [Header("the section to load")]
    public string sectionToLoad;

    [Header("Visual marker for current training path")]
    public Text text_youAreIn;

    OnClickManager clickManager;
    GameObject buttonContainer;
    Section [] sections;

    public string[] titles;
    public UI_ContentDisplayManager trainingPath_contentNAMES;



    // Use this for initialization
    void Start()
    {
        clickManager = GetComponent<OnClickManager>();
        sections = GetComponentsInChildren<Section>();

        trainingPath_contentNAMES = GameObject.FindGameObjectWithTag("AppManager").GetComponent<UI_ContentDisplayManager>();

    }


    public void retrieveNumberOfElements(string nameSection) {
        int i= 0;
        titles = new string[clickManager.amountOfVideo_buttons];

        titles = new string[trainingPath_contentNAMES.titles.Length];
        titles = trainingPath_contentNAMES.titles;


        foreach (Section singleSection in sections) {
            // Debug.Log("hola mundo");
            if (singleSection.SectionName == nameSection) {
                singleSection.loadElementsFromSectionPath();
                clickManager.amountOfVideo_buttons = singleSection.numbOfElementsinPath;
                //Debug.Log(clickManager.amountOfVideo_buttons);
                foreach (string title in trainingPath_contentNAMES.titles) {
                    Debug.Log(trainingPath_contentNAMES.titles[i]);
                    break;
                }                
                clickManager.createVideoButton_container(clickManager.amountOfVideo_buttons /*, titles[i]*/);                    
            }
            i++;
        }
    }


    public void youAreinSection(string section) {
        sectionToLoad = section;
        text_youAreIn.text = sectionToLoad;
        //Debug.Log(section);
        retrieveNumberOfElements(section);
    }


    public void loadFromThisPath(string path )
    {
        PathReceived = path;
        clickManager.pathChoosed = PathReceived;
        
        //path choosed
        //Debug.Log(path);
    }

 

}
