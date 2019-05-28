using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using Newtonsoft.Json;


public class JSON_utilities : MonoBehaviour {
        StreamReader reader;
        StreamWriter writer;
        JsonSerializer serializer;

        public int ID = 1;
        public bool canRead;
        public string path ;
        public string json;

        public video clipInfo;
        public video clipInfoJSON_deserialized;


        public int setNextID()
        {
            return ID++;
        }


        public string createFileAndWrite(string path, Video temp)
        {
            using (StreamWriter sw = (File.Exists(path)) ? File.AppendText(path) : File.CreateText(path))
            {                
                json = JsonConvert.SerializeObject(temp);
                sw.WriteLine(json);
                return json;
            }
        }  

    public string readAllDataFromFile(string path)
        {
            string temp_json = string.Empty;
            using (System.IO.StreamReader reader = new System.IO.StreamReader(path, Encoding.UTF8))
            {
                temp_json = reader.ReadToEnd();
                return temp_json;
            }
        }

    private void Start()
    {
        create_JSON_Information();
    }

    public void create_JSON_Information()
    {
        Video_List list = new Video_List();

        int i = 0;
        var asset = Resources.Load<TextAsset>(path);

        
        
        string temp_json = asset.text;

       list = JsonConvert.DeserializeObject<Video_List>(temp_json);

            Debug.Log(list.video.Count);

        JsonTextReader reader = new JsonTextReader(new StringReader(temp_json));
        reader.SupportMultipleContent = true;

        while (true) {
            if (!reader.Read()) {
                break;
            }
            try
            {

                JsonSerializer serializer = new JsonSerializer();
                video infoVideo = serializer.Deserialize<video>(reader);

                //list = JsonUtility.FromJson<Video_List>(temp_json);


                list.video.Add(infoVideo);            
                
                foreach (video json_element in list.video)
                {
                    list.video.Add(json_element);
                    Debug.Log(json_element.Title);
                    Debug.Log(json_element.Description);
                    Debug.Log(json_element.Section);                
                }
                
            }
            catch (ArgumentException e)
            {
                Debug.LogError(e.ToString());

            }

        }


        // return list;
        //print for DEBUG
       /*
        foreach (video infoVideo in list.videos) {
            Debug.Log(infoVideo.Title);
            Debug.Log(infoVideo.Section);
            Debug.Log(infoVideo.Description);           
        }
        */
    }




}


