using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class CreateRecipientArray : MonoBehaviour
{
    [Header("Amount of video recipients")]
    [SerializeField, Range(0, 100)]
    public int cantCubes;

    [Header("Array of recipients")]
    public GameObject[] cubes;

    [Header("Prefab used to create recipients")]
    public GameObject cubePrefab;

    [Header("Flags to control array creation flow")]
    public bool canCreateCubes = false;
    public bool isArrayCreated = false;
    public Vector3 cubePosOffset;

    [Header("Materials used")]
    public Material Selected;
    public Material Highlighted;

    [Header("Selected position, through array")]
    public int posSelected;

    public GameObject CubeParent;
    public Transform[] childs;
    public RecipientAttributes[] attribute;

    videoArrayManager videoManager;
    Video videoP;


    #region initialize array
    /// <summary>
    /// this method receives directly the amount of video recipients to create.
    /// </summary>
    /// <param name="amount"> the amount of video recipients defined by a local search in folder</param>
    /// <returns>the amount of video recipients used to create the array</returns>
    public int AmountOfCubes(int amount)
    {
        cantCubes = amount;
        return cantCubes;
    }
    /// <summary>
    /// this method initialize the array with the exact amount of video recipients.
    /// </summary>
    public void InitializeCubeArray(int cant)
    {
        cubes = new GameObject[cant];
    }
    #endregion


    #region assing videos to recipients.
    public void assignVideoInRecipient(int index, GameObject temp_recipient)
    {
        temp_recipient.GetComponent<Video>().Nombre = videoManager.NamesVideos[index].ToString();
        temp_recipient.GetComponent<Video>().clip = videoManager.Local_videoClips[index];
        temp_recipient.GetComponent<Video>().URL = Application.dataPath + "/Resources/" + videoManager.local_folder; /*.mp4,.mov,.avi*/
        //tempRecipient.GetComponent<VideoPlayer>().clip = videoManager.Local_videoClips[index];
    }
    #endregion

    public void AssignMaterialToRecipient(int index, GameObject temp_recipient) {
        temp_recipient.GetComponent<Renderer>().sharedMaterial = Selected;
    }

    #region Instantiate cubes
    public void CreateCubes(int j)
    {
        cubePosOffset = new Vector3(CubeParent.transform.position.x, CubeParent.transform.position.y, CubeParent.transform.position.z + (j * 1.5f));
        GameObject temp = Instantiate(cubePrefab, cubePosOffset, CubeParent.transform.rotation) as GameObject;
        temp.transform.parent = CubeParent.transform; //parent under NodeParent
        temp.tag = "VideoCube";                       //assign TAG to each sphere  
        temp.name = "cube_" + j;                      //assign name to each sphere according to order
        assignVideoInRecipient(j, temp);
        temp.GetComponent<RecipientAttributes>().positionAssingned = j; //assign position to each sphere created
        temp.GetComponent<LerpingActions>().lerpSpeed = 0.3f; //speed to lerp to center once selected;
        temp.AddComponent<videoRecipientController>();
        //temp.GetComponent<Renderer>().material  
        //temp.AddComponent<VideoPlayerManager>();//add video player to eachSphere
        //temp.GetComponent<VideoPlayer>().playOnAwake = false;
        //temp.GetComponent<VideoPlayer>().waitForFirstFrame  = false;
        cubes[j] = temp;
    }
    #endregion

    #region loop Create Arrays
    public void CreateArrayOfCubes() {
        if (GameObject.FindGameObjectsWithTag("VideoCube").Length < cantCubes)
        {
            for (int i = 0; i < cantCubes; i++)
            {
                CreateCubes(i);
            }
        }else {
            return;
        }
    }
    #endregion

   

    #region validate if can Create arrays
    public void CanIcreateCubes() {
        if (GameObject.FindGameObjectsWithTag("VideoCube").Length < cantCubes)//if this amount is small than amount allowed
        {
            canCreateCubes = true;
        }
        else {
            canCreateCubes = false;
        }
    }
    #endregion

    #region validate if array is created
    public bool IsArrayCreated() {        
        if (GameObject.FindGameObjectsWithTag("VideoCube").Length == cantCubes)//if this amount is small than amount allowed
        {
            isArrayCreated = true;
            // StartCoroutine("action");
            return isArrayCreated;
        }
        else
        {
            return isArrayCreated = false;
        }
        return isArrayCreated;
    }
    #endregion

    #region Initialize stuff
    public void AppStarter(/*int cantRecipients*/) {
        videoManager = GetComponent<videoArrayManager>();
        cantCubes = videoManager.amountOfVideos;

        InitializeCubeArray(cantCubes);
        childs = CubeParent.GetComponentsInChildren<Transform>();
        attribute = new RecipientAttributes[cantCubes];
    }
    #endregion

    void Start()
    {      
        
        AppStarter(/*cantCubes*/);
        // Debug.Log("cantidad de recipientes de video a crear: " + cantCubes);
    }

    #region loop logic
    void Update()
    {

        //CanIcreateCubes(videoManager.amountOfVideos);
        IsArrayCreated();
        CreateArrayOfCubes();
    }
    #endregion
}
