using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class JSON_Handler : MonoBehaviour {


    //public TextMeshProUGUI textM;
    video UI_info;
    [Header("Path to the sections json File")]
    public string sectionsPath = "config/sections/";

    string m_Path;
    [Header("Number of UI Section buttons to create")]
    public int numbOfSectionButtons;

    [Header("Names for the sections")]
    public string[] sectionNames;

    // [Header("List of objects")]
    Video_List json_list = new Video_List();
    Video_List [] jsonlist;

    
       
    TextAsset[] resources;
    
    int resourcesSize;

    UI_ResourcesArray assetArray = new UI_ResourcesArray();


    void Awake() {

        #region nothing in use
        /*  jsonUtil = GetComponent<JSON_utilities>();

          //Get the path of the Game data folder
          m_Path = Application.dataPath;
          m_Path = m_Path + jsonUtil.path;*/


        /*
        jsonUtil.createFileAndWrite(m_Path, null);        
        textM.text = jsonUtil.readAllDataFromFile(m_Path);
        */
        // textM = GameObject.FindGameObjectWithTag("UI_textContainer").GetComponent<TextMeshProUGUI>();
        //Debug.Log("Path : " + m_Path);

        //createUIObjects();
        #endregion

        getResourcesCount();
        determineNumberOfSections();
        determineSectionNames();
       
    }


    #region count the number of resources when loading  from folder
    public void getResourcesCount()
    {
        string resourcesFolderPath = sectionsPath;// "config/sections/";
        //Load all resources from folder
        resources = Resources.LoadAll<TextAsset>(resourcesFolderPath);

        //determine size of the resources found
        resourcesSize = resources.Length;

        //create array of arrays of objects made by json text files.
        assetArray.Asset = new TextAsset[resourcesSize];

        jsonlist = new Video_List[assetArray.Asset.Length];
        sectionNames = new string[assetArray.Asset.Length];

        //temp_VideoNamesFromSection = new string[assetArray.Asset.Length];

        storeIndividualResources();
    }
    #endregion
    
    #region setting up the number of sections 
    public int determineNumberOfSections()
    {
        return numbOfSectionButtons = assetArray.Asset.Length;
    }
    #endregion


    #region Determine the number of Elements in each of the loaded sections
    public int determineNumberOfElmsBySection() {
        int numOfElems=0;

        return numOfElems;
    }
    #endregion

    #region Store individual resources inside a Data array
    public void storeIndividualResources() {
        int i = 0;
        foreach (TextAsset resource in resources)
        {
            assetArray.Asset[i] = resource;
            
            //print array of arrays of objects from each json file;
            //Debug.Log(assetArray.Asset[i]);       
            //ProcessSections(assetArray.Asset[i]);
            i++;
        }

        //Debug.Log(assetArray.Asset.Length);

    }
    #endregion

    #region Determine the section names and insert it into an ARRAY
    public void determineSectionNames() {
        TextAsset temp= null;
        int i = 0;
                
        foreach (TextAsset asset in assetArray.Asset) {
            // Debug.Log(asset);
            temp = asset;
            ProcessSections(temp.text, i);
           // Debug.Log("numero de vuelta: " +  i);
            i++;
        }
        

    }




    public void ProcessSections(string asset, int i)
    {
            //Debug.Log(asset);
            
            jsonlist[i] = JsonConvert.DeserializeObject<Video_List>(asset);          

            foreach (video jsonElement in jsonlist[i].video)
            {
            extractName(jsonElement.Section.ToString(), i);
            //Debug.Log(jsonElement.Section.ToString());
            //readVideoNamesFromSection(jsonElement.Title.ToString(), i);
            return;            
            }              
    }


    public void extractName(string tempName, int i)
    {        
        sectionNames[i] = tempName;
            //Debug.Log(i + "----" + sectionNames[i]);
    }
    #endregion





    /***************************************************************************************************/
    /***************************************************************************************************/
    /***************************************************************************************************/
    #region Use this as template for JSON PROCESING
    //******************************JSON SKELETN****************************************************//


    string path = "config/config";
    Text text;

    public void createUIObjects(TextAsset asset) {
        int elementIndex = 0;
         //  var asset = Resources.Load<TextAsset>(path);


            if (asset != null)
            {

                Debug.Log("contenido procesando...");

            json_list = JsonConvert.DeserializeObject<Video_List>(asset.text);
            foreach (video json_element in json_list.video)
                {

                Debug.Log("Elemento: " + elementIndex);
                Debug.Log(json_element.Title);
                Debug.Log(json_element.Description);
                Debug.Log(json_element.Section);
                
                
                  text.text  = json_element.Title.ToString() +
                  json_element.Description.ToString() + 
                  json_element.Section.ToString();
                  
                elementIndex++;                    
                }
            }
            else {
                Debug.Log("File is Null");
            }
            
    }
    #endregion

 }
