using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class JSON_SectionInfo : MonoBehaviour {

    Video info;
    [Header("Path to the section file")]
    public string sectionConfigFile_path = "config/sections/";

    [Header("-------------------------------------------------")]
    Video_List json_list = new Video_List();

    [Header("List of videoList objects")]
    Video_List[] jsonList;

    [SerializeField]
    [Header("-------------------------------------------------")]
    [Header("Resources for this section")]
    public TextAsset assignedResource;

    [Header("-------------------------------------------------")]
    [Header("Resources array loaded")]
    public TextAsset[] resources;
    [Header("Amount of resources loaded")]
    public int  resourceSize;
    Section section;
    public string tempName;
    [Header("-------------------------------------------------")]
    [Header("section Info Elements")]
    public string[] titles;
    public string[] description;

    public UI_ContentDisplayManager appManager;


    void Start()
    {
        section = GetComponent<Section>();
        resources = Resources.LoadAll<TextAsset>(sectionConfigFile_path);
        retrieveNameOfSection();
        DefineResourcesForThisSection();

        appManager = GameObject.FindGameObjectWithTag("AppManager").GetComponent<UI_ContentDisplayManager>();
    }

    public void retrieveNameOfSection() {        
        tempName = section.SectionName;
        tempName = tempName.Replace(" ", string.Empty);
        tempName = tempName.ToLower();
     //Debug.Log("esta es la seccion: " + tempName);
    }

 



    public void DefineResourcesForThisSection()
    {
        int elementIndex = 0;
        int index = 0;
        resourceSize = resources.Length;

        foreach (TextAsset resource in resources)
        {
            if (resource.name == tempName)
            {
                assignedResource = resource;

                json_list = JsonConvert.DeserializeObject<Video_List>(assignedResource.text);

                foreach (video json_element in json_list.video)
                {

                    // titles[elementIndex] = json_element.Title;
                    //description[elementIndex] = json_element.Description;


                    //Debug.Log("Seccion: "+json_element.Section);
                    //Debug.Log("Titulo:  "+json_element.Title);
                    //Debug.Log("Descripcion: "+json_element.Description);

                    elementIndex++;
                }
                index++;
                initializeInfoArrays(elementIndex);
            }
        }
    }




    public void defineResourcesForThisSection()
    {
        int elementIndex = 0;
        int index = 0;
        resourceSize = resources.Length;
        TextAsset tempAsset;
        for (int j = 0; j < resourceSize; j++)
        {
            Debug.Log("assigned resource: " + resources[j].name);

            if (tempName == resources[j].name)
            {
                assignedResource = resources[j];
                


                        json_list = JsonConvert.DeserializeObject<Video_List>(assignedResource.text);

                        foreach (video json_element in json_list.video)
                        {

                            //titles[elementIndex] = json_element.Title;
                            //description[elementIndex] = json_element.Description;


                            //Debug.Log("Seccion: "+json_element.Section);
                            //Debug.Log("Titulo:  "+json_element.Title);
                            //Debug.Log("Descripcion: "+json_element.Description);

                            elementIndex++;
                        }
                index++;
             initializeInfoArrays(elementIndex);
            }
        }
    }





    /*
    foreach (TextAsset resource in resources) {
        if ( resource.name  ==  tempName  ) {
            assignedResource = resource;

                json_list = JsonConvert.DeserializeObject<Video_List>(assignedResource.text);

            foreach (video json_element in json_list.video)
                {

                   // titles[elementIndex] = json_element.Title;
                    //description[elementIndex] = json_element.Description;


                    //Debug.Log("Seccion: "+json_element.Section);
                    //Debug.Log("Titulo:  "+json_element.Title);
                    //Debug.Log("Descripcion: "+json_element.Description);

                elementIndex++;
                }
            index++;
            initializeInfoArrays(elementIndex);
        }
    }*/


/*
    private void Update()
    {
        retrieveNameOfSection();
        DefineResourcesForThisSection();

    }
    */


    public void initializeInfoArrays(int size) {
        titles = new string[size];
        description = new string[size];
    }

    public void broadCastTextAssetElement() {
     appManager.assignResource(assignedResource);
    }








}
