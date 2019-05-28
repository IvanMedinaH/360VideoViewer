using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class InfoSection : MonoBehaviour {

    [Header("Path to names")]
    public string paths;
    [Header("-------------------------")]

    [Header("Training path information")]
    [SerializeField]
    private string numOfElementsInSection;
    [Header("-------------------------")]
    public TextAsset asset;
    public string[] titles;
    public string[] section;
    public string[] description;
    public string[] ID;
    
  

    [Header("-------------------------")]
    public TextAsset[] resources;

    public JSON_Handler json_section;
    Video_List json_list = new Video_List();


    // Use this for initialization
    void Start() {
        json_section = GameObject.FindGameObjectWithTag("AppManager").GetComponent<JSON_Handler>();
        paths = json_section.sectionsPath;
    }


    // Update is called once per frame
    void Update() {
        retrieveDataFromPath();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }
    public void retrieveDataFromPath() {
      resources = Resources.LoadAll<TextAsset>(paths);


      
            int i = 0;
            foreach (TextAsset resource in resources)
            {
                //assetArray.Asset[i] = resource;

                //print array of arrays of objects from each json file;
                //Debug.Log(assetArray.Asset[i]);       
                //ProcessSections(assetArray.Asset[i]);
                i++;
            }

            //Debug.Log(assetArray.Asset.Length);     

    }

    

    public void getValueForTextAsset( TextAsset tempAsset, string path){
        int elementIndex = 0;
        var asset = Resources.Load<TextAsset>(path);


        if (asset != null)
        {

            Debug.Log("contenido procesando...");

            json_list = JsonConvert.DeserializeObject<Video_List>(tempAsset.text);
            foreach (video json_element in json_list.video)
            {

                Debug.Log("Elemento: " + elementIndex);
                Debug.Log(json_element.Title);
                Debug.Log(json_element.Description);
                Debug.Log(json_element.Section);

                elementIndex++;
            }
        }
        else
        {
            Debug.Log("File is Null");
        }
    }
}
