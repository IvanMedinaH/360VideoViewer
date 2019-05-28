using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class JSON_SectionConfig : MonoBehaviour {




    [Header("Array of objects inside section")]
    public int numbVideoObjects;

    [Header("List of paths")]
    [SerializeField]
    Path_List listOfPaths;

    [Header("Path to json config file: ")]
    public string jsonConfigPath;

    [Header("Path to json video names in this section: ")]
    public string videoNames_jsonConfigPath;


    [Header("The paths to choose")]
    public string[] Listpaths;

    void Start () {
        jsonConfigPath= "config/paths/paths";

        retrieveInfo();
    }	




    public void retrieveInfo() {

        int elementIndex = 0;
        TextAsset asset = Resources.Load<TextAsset>(jsonConfigPath);
            //debug assets text from json Paths
            //Debug.Log(asset.text);
        
        if (asset != null)
        {

            listOfPaths = new Path_List();
            listOfPaths = JsonConvert.DeserializeObject<Path_List>(asset.text);
            Listpaths = new string[listOfPaths.Paths.Count];
                //number of paths retrieved
                //Debug.Log(listOfPaths.Paths.Count);

            foreach (Paths path in listOfPaths.Paths) 
            {
                /*
                Debug.Log("Elemento: " + elementIndex);
                Debug.Log(path.Path);
                Debug.Log(path.SectionName);
                */
                Listpaths[elementIndex] = path.Path;
               /* text.text = path.Path.ToString() +" "+
                path.SectionName.ToString();
               */
                elementIndex++;
            }
        }
        else
        {
            Debug.Log("File is Null");
        }
        
    }

    public void retrieveElementNames() {

    }


    //tesgin 



}
