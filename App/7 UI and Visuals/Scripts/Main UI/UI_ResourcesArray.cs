using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ResourcesArray: MonoBehaviour{
    private TextAsset[] asset;

    public TextAsset[] Asset
    {
        get
        {
            return asset;
        }

        set
        {
            asset = value;
        }
    }
}
