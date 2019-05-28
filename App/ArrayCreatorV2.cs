using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayCreatorV2 : MonoBehaviour
{

    [SerializeField, Range(0, 100)]
    public int cantCubes;
    public GameObject[] cubes;
    public GameObject cubePrefab;
    public bool canCreateCubes = false;
    public bool isArrayCreated = false;
    public Vector3 cubePosOffset;
    public Material selected;
    public Material notSelected;
    public int posSelected;

    public GameObject CubeParent;
    public Transform[] childs;
    public RecipientAttributes[] attribute;
    videoArrayManager videoManager;



    public void InitializeCubeArray()
    {
        cubes = new GameObject[videoManager.amountOfVideos];
    }

    public int AmountOfCubes()
    {
        int numb = cantCubes;
        return numb;
    }

    public void CreateCubes(int j)
    {
        cubePosOffset = new Vector3(CubeParent.transform.position.x, CubeParent.transform.position.y, CubeParent.transform.position.z + (j * 1.5f));
        GameObject temp = Instantiate(cubePrefab, cubePosOffset, CubeParent.transform.rotation) as GameObject;
        temp.transform.parent = CubeParent.transform; //parent under NodeParent
        temp.tag = "VideoCube";                       //assign TAG to each sphere  
        temp.name = "cube_" + j;                      //assign name to each sphere according to order
        temp.GetComponent<RecipientAttributes>().positionAssingned = j; //assign position to each sphere created
        temp.AddComponent<VideoPlayerManager>();//add video player to eachSphere

        cubes[j] = temp;
    }

    public void CreateArrayOfCubes(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            CreateCubes(i);
        }
    }

    public void CanIcreateCubes()
    {
        int temp_cant = videoManager.amountOfVideos;//amount of VideoRecipients allowed in the scene
        if (GameObject.FindGameObjectsWithTag("VideoCube").Length < temp_cant)//if this amount is small than amount allowed
        {
            canCreateCubes = true;
        }
        else
        {
            canCreateCubes = false;
        }
    }
    public bool IsArrayCreated()
    {
        int temp_cant = videoManager.amountOfVideos;//amount of VideoRecipients allowed in the scene
        if (GameObject.FindGameObjectsWithTag("VideoCube").Length == temp_cant)//if this amount is small than amount allowed
        {
            StartCoroutine("action");
            return isArrayCreated = true;
        }
        else
        {
            return isArrayCreated = false;
        }
        return isArrayCreated;
    }

    public void AppStarter()
    {
        videoManager = GetComponent<videoArrayManager>();
        //checkPositionsFromArray();
        InitializeCubeArray();
        childs = CubeParent.GetComponentsInChildren<Transform>();
        attribute = new RecipientAttributes[childs.Length];
    }

    void Start()
    {
        videoManager = GetComponent<videoArrayManager>();
        if (videoManager.amountOfVideos != 0 && canCreateCubes)
        {
            //checkPositionsFromArray();
            InitializeCubeArray();
            childs = CubeParent.GetComponentsInChildren<Transform>();
            attribute = new RecipientAttributes[childs.Length];



        }

    }

    IEnumerator action()
    {
        while (cantCubes < videoManager.amountOfVideos)
        {
            yield return new WaitForSeconds(1.0f);
            CreateArrayOfCubes(videoManager.amountOfVideos);
        }
    }


    void Update()
    {
        CanIcreateCubes();
        AmountOfCubes();
        IsArrayCreated();
    }
}
