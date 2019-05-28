using UnityEngine;
using UnityEditor;
using System.Collections;
using EasyInputVR.Core;

public static class EasyInputMenu
{

    [MenuItem("GameObject/Easy Input Helper/Add Easy Input Helper")]
    static void AddEasyInputHelper()
    {

        if (GameObject.FindObjectOfType<EasyInputHelper>() == null)
        {
            GameObject inputHelper;
            MonoScript script;
            inputHelper = new GameObject("EasyInputHelper", typeof(EasyInputHelper));
            script = MonoScript.FromMonoBehaviour(inputHelper.GetComponent<EasyInputHelper>());
            MonoImporter.SetExecutionOrder(script, -100);
        }
        else
        {
            EditorUtility.DisplayDialog("Warning", "EasyInputHelper already exists in your scene", "OK");
        }

        Selection.activeObject = GameObject.FindObjectOfType<EasyInputHelper>().gameObject;
    }

}
