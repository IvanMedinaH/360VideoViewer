using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Paths  {

  private string sectionName;
  private string path;

    public Paths() { }

    public string SectionName
    {
        get
        {
            return sectionName;
        }

        set
        {
            sectionName = value;
        }
    }
    public string Path
    {
        get
        {
            return path;
        }

        set
        {
            path = value;
        }
    }


}
