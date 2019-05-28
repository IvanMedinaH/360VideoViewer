using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;
using System.Linq;

public class VideoManagerV2 : MonoBehaviour
{

    public Video[] Videos;
    private VideoClip clip;

    //public VideoPlayer player;
    public GameObject Selected_player360;

    public int amountOfVideos = 0;
    public object[] videoClips;
    public string[] NamesVideos;



    public void AssignVideo(int position)
    {
        foreach (Video video in Videos)
        {

        }
    }

    public void CreateArrayOfVideos()
    {

    }

    public void Start()
    {
        NamesVideos = new string[amountOfVideos];
        videoClips = new GameObject[amountOfVideos];
        ReadResourcesVideoFolder();
    }



    public void ReadResourcesVideoFolder()
    {
        amountOfVideos = Resources.LoadAll<VideoClip>("360videos/").Length;
        videoClips = Resources.LoadAll<VideoClip>("360videos/");
    }

    public void GetVideoNames()
    {
        int i = 0;
        GameObject[] tempObj = new GameObject[amountOfVideos];

        tempObj = (GameObject[])videoClips;

        for (int j = 0; j < amountOfVideos; j++)
        {
            NamesVideos[i] = tempObj[j].name;
            Debug.Log(NamesVideos[i]);
        }

        foreach (GameObject video in videoClips)
        {

            NamesVideos[i] = video.name;

            i++;
        }
    }


}
