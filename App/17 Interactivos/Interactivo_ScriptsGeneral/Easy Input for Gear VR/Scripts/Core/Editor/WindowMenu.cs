using UnityEngine;
using UnityEditor;
using System.Collections;
using EasyInputVR.Core;

public static class WindowMenu
{

    [MenuItem("Window/EasyInputGearVR/Move Controller Model to Prefab")]
    static void GearVRBuild()
    {
        if (!AssetDatabase.IsValidFolder("Assets/OVR"))
        {
            EditorUtility.DisplayDialog("Oculus Utilities not imported", "You must have the Oculus Utilities imported in order to move the controller model into a prefab", "Ok");
            return;
        }


        //create gear vr controller
        //check to see if exists already
        object tempA = EditorGUIUtility.Load("Assets/Easy Input for Gear VR/Resources/gearvrcontroller.prefab");
        if (tempA == null)
        {
            //means we haven't created our prefab in resources to include in build
            PrefabUtility.CreatePrefab("Assets/Easy Input for Gear VR/Resources/gearvrcontroller.prefab", (GameObject)EditorGUIUtility.Load("Assets/OVR/Meshes/GearVrController/GearVrController.fbx"));

        }
    }

    [MenuItem("Window/EasyInputGearVR/Show Recommended Settings")]
    static void ShowRecommendedSettings()
    {
        var window = EditorWindow.GetWindow<GearVRSettings>(true);
        window.minSize = new Vector2(640, 480);
    }
}
