using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_sectionGenerator_Manager : MonoBehaviour {
    [Header("The number of sections to create: ")]
    public int numberOfSectionsToCreate = 0;

    [Header("The data source: ")]
    public JSON_Handler section_DataSource;

    [Header("The button prefab to instantiate: ")]
    public GameObject section_Button;

    [Header("The section container: ")]
    public RectTransform sectionContainerParent;

    UI_SectionHandler sectionHandler;
    

    public RectTransform mainCanvas;
    public RectTransform trainingPathCanvas;
    GameObject datasource;
    //-------------------------------------------------------
    //public UI_sectionGenerator_Manager() {
    //    InitializeObjects();
    //}

    public void InitializeObjects() {
        section_DataSource.GetComponent<JSON_Handler>();
        sectionHandler = GameObject.FindGameObjectWithTag("mainUI").GetComponent<UI_SectionHandler>();
        createAndAttachSectionButtons();
        sectionContainerParent.GetComponent<GameObject>().SetActive(true);
    }


    void Start()
    {
        section_DataSource.GetComponent<JSON_Handler>();
        sectionHandler = GameObject.FindGameObjectWithTag("mainUI").GetComponent<UI_SectionHandler>();
        createAndAttachSectionButtons();
    }


    private void Update()
    {
        if (sectionContainerParent.childCount < numberOfSectionsToCreate)
        {
          createAndAttachSectionButtons();
        }
        else
        {
            return;
        }        
    }


    public void createAndAttachSectionButtons() {
        numberOfSectionsToCreate = section_DataSource.sectionNames.Length;
         
        string [] temp_videoName  = new  string [numberOfSectionsToCreate];

        for (int i = 0; i < numberOfSectionsToCreate; i++) {

            //Names from JSON equal to names in variable array
            temp_videoName[i] = section_DataSource.sectionNames[i];
           // Debug.Log(i + "----" + temp_videoName[i]);

            GameObject sectBtn_cln = (GameObject)Instantiate(section_Button, sectionContainerParent);
            sectBtn_cln.transform.SetParent(sectionContainerParent,false);


            //adding --->SECTION COMPONENTS to section button
            sectBtn_cln.AddComponent<Section>();           
            sectBtn_cln.AddComponent<MainBtn_Section>();
            sectBtn_cln.AddComponent<JSON_SectionInfo>();


            //sectBtn_cln.AddComponent<JSON_SectionInfo>();

            //Getting --->SECTION COMPONENTS to section button
            sectBtn_cln.GetComponent<JSON_SectionInfo>().sectionConfigFile_path = "config/sections/";
            sectBtn_cln.GetComponent<Section>().SectionName = temp_videoName[i];


            /*tmpro*/
            sectBtn_cln.name = sectBtn_cln.GetComponent<Section>().SectionName;
            //sectBtn_cln.GetComponent<Section>().NameTextUI.text = sectBtn_cln.GetComponent<Section>().SectionName;
            sectBtn_cln.GetComponent<Section>().sectionName.SetText( sectBtn_cln.GetComponent<Section>().SectionName);


            Button tempButton = sectBtn_cln.GetComponent<Button>();           
            //adding --->LISTENERS to section buttons
            tempButton.onClick.AddListener(() => mainCanvas.GetComponent<canvasElement>().deactivateMain("main_canvas"));
            tempButton.onClick.AddListener(() => trainingPathCanvas.GetComponent<canvasElement>().activateTrainingpath("trainingPath_canvas"));
            /*
            tempButton.onClick.AddListener(() => loadThisSection(sectBtn_cln.GetComponent<Section>().SectionName));
            tempButton.onClick.AddListener(() => loadThisSection(sectBtn_cln.GetComponent<Section>().sectionPath));
            */
            tempButton.onClick.AddListener(() => sectionHandler.loadFromThisPath(sectBtn_cln.GetComponent<Section>().sectionPath));
            tempButton.onClick.AddListener(() => sectionHandler.youAreinSection(sectBtn_cln.GetComponent<Section>().SectionName));
            tempButton.onClick.AddListener(() => sectBtn_cln.GetComponent<Section>().loadElementsFromSectionPath());        

            tempButton.onClick.AddListener(() => sectBtn_cln.GetComponent<MainBtn_Section>().deactivateThisPanel());            
            tempButton.onClick.AddListener(() => sectBtn_cln.GetComponent<JSON_SectionInfo>().broadCastTextAssetElement());
        }
    }

    /*
    public void sortChilds(string namebtnSection) {
        if (this.SectionName.text != namebtnSection)
        {
            this.SectionName.text = namebtnSection;
        }
        else {
            return;
        }
    }

    public void setNameThisComponent(string Name, GameObject clone, int i) {
        clone.name = Name;
        this.SectionName.text = i.ToString();
    }
    */



    void onClicked(string buttonName)    {
        //EventManager.TriggerEvent("loadVideoIn360VR");
        Debug.Log("Button clicked = " + buttonName);
    }

    /*
    public void loadThisSection(string sectionName) {
        Debug.Log("Section: " + sectionName);
    }
    */
    public void loadThisAmount()    {
        int amount = 0;
        amount  = this.gameObject.GetComponent<Section>().loadElementsFromSectionPath();
        Debug.Log(amount);
     }
    



}
