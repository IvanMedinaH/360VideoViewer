using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TrainingPathContentmonitor : MonoBehaviour
{

    public GameObject[] videoElement;
    public string[] titles;
    // Use this for initialization
    UI_ContentDisplayManager appManager;

    void Start()
    {
        appManager = GameObject.FindGameObjectWithTag("AppManager").GetComponent<UI_ContentDisplayManager>();
        titles = appManager.titles;
    }

    public void collectChildsFromTrainingPathContent()
    {
        int i = 0;
        videoElement = GameObject.FindGameObjectsWithTag("TitleDisplay");

        foreach (GameObject child in videoElement)
        {
            if (child.tag == "TitleDisplay")
            {
                child.GetComponent<TextMeshPro>().SetText(appManager.titles[i]); //= titles[i].ToString();
            }
            i++;
        }

    }


    // Update is called once per frame
    void Update()
    {
        collectChildsFromTrainingPathContent();
        if (appManager.isCanvasElementOn)
        {
            titles = appManager.titles;

        }
        else
        {
            titles = null;
        }
    }
}