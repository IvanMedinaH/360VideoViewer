using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;

public class videoArrayManager : MonoBehaviour {

    public Video[] Videos;
    private VideoClip clip;

    //public VideoPlayer player;
    public GameObject  Selected_player360;
    public int amountOfVideos;
    public VideoClip [] Local_videoClips;
    public VideoClip[] Network_videoClips;
    public ArrayList  NamesVideos;

    

    public string local_folder = "360videos/";
    public string Network_folder = "";
    string IP_address="192.168.50.16";
    string Port = ":8080";
    string location = "Unitytest/360videos/";
    
    GameObject[] tempObj;


    #region Initialize all resources
    public void InitializeManager(){
        NamesVideos = new ArrayList();
        Local_videoClips = new VideoClip[amountOfVideos];
        tempObj = new GameObject[amountOfVideos];
        amountOfVideos = Local_videoClips.Length;
        amountOfVideos = LoadLocalResources_VideoFolder(local_folder);
        //amountOfVideos = LoadNetworkResources_VideoFolder(Network_folder);
    }
    #endregion
    
    public void Awake(){
        InitializeManager();
        GetVideoNames();
    }


    #region load from NETWORK
    public int LoadNetworkResources_VideoFolder(string url) {
        int cantVideos;
        Network_videoClips = Resources.LoadAll<VideoClip>(url);
        cantVideos = Network_videoClips.Length;
        return cantVideos;
    }
    #endregion

    
    #region load from Local
    public int LoadLocalResources_VideoFolder(string name){
        int cantVideos;
        
        Local_videoClips = Resources.LoadAll<VideoClip>(name);
        cantVideos = Local_videoClips.Length;
        return cantVideos;
    }
    #endregion


    #region retrieve NAMES from videos
    public void GetVideoNames()    {
        int i = 0;
        string temp = null;
        foreach (VideoClip video in Local_videoClips)
        {
            //Debug.Log(video.name); 
            NamesVideos.Add(video.name);
            i++;
        }
    }
    #endregion


}
