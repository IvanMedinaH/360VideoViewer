using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class UI_ContentDisplayManager : MonoBehaviour {

    public canvasElement TrainingPath;
    public bool isCanvasElementOn;
    public TextAsset infoResourceToLoad;
    Video_List json_list = new Video_List();
    [Header("---------------------------------")]
    [Header("Titulos para la seccion elegida: ")]
    public string [] titles;



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        isTrainingPathOn();
	}

    public void isTrainingPathOn() {
        if (TrainingPath.isActive)
        {
            isCanvasElementOn = true;
        }
        else {
            isCanvasElementOn = false;
        }
    }
    public void resetResource() {
        infoResourceToLoad = null;
    }

    public void AssignAResource(TextAsset newResource) {
        infoResourceToLoad = newResource;
    }


    public void assignResource(TextAsset resourceAssigned) {
        infoResourceToLoad = resourceAssigned;

        Debug.Log("TextAsset: "+ resourceAssigned);

        int elementIndex = 0;
            //  var asset = Resources.Load<TextAsset>(path);


            if (resourceAssigned != null)
            {
                //Resource assigned from folder
                //Debug.Log("contenido procesando...");

                json_list = JsonConvert.DeserializeObject<Video_List>(resourceAssigned.text);

                titles = new string[json_list.video.Count];
                foreach (video json_element in json_list.video)
                {
                titles[elementIndex] = json_element.Title;
                /*
                    Debug.Log("Elemento: " + elementIndex);
                    Debug.Log(json_element.Title);
                    Debug.Log(json_element.Description);
                    Debug.Log(json_element.Section);
               
                */
                    elementIndex++;
                }
            }
            else
            {
                Debug.Log("File is Null");
            }
    }

  
    


}
