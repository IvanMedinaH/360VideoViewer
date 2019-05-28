using UnityEngine;
using UnityEditor;
using System.IO;

[InitializeOnLoad]
public class GearVRSettings : EditorWindow
{

    const string dismiss = "dismiss-";
    const string useSuggested = "Use Suggested";

    const string buildTarget = "Build Target (android recommended)";
    const string virRealitySupported = "Virtual Reality Supported (true recommended)";
    const string minimumAndroidSdk = "Minumum Supported Android SDK (level 21 recommended)";
    const string oculussdk = "Oculus Utilites aren't downloaded and imported (required for Gear VR)";
    const string oculussig = "Oculus Signature file isn't provided (required for Gear VR testing)";



    const BuildTarget myBuildTarget = BuildTarget.Android;
    const bool virtualRealitySupported = true;
    const int minAndroidSdk = 21;

    static GearVRSettings window;

    static GearVRSettings()
    {
        EditorApplication.update += Update;
    }

    static void Update()
    {
        //when updating from version 1.0 to 1.1 an existing script needed to get renamed
        //but asset import isn't automatically renaming it (so look for the old name and if it exists delete it)
        //AssetDatabase.DeleteAsset("Assets/Easy Input for Gear VR/Scripts/Standard Controllers/StandardGearVRControllerFollowWithRay.cs");

        //this is the hook for initialize on load not if opening through the build menu
        //this function decides if we should show the window when starting or importing
        //basically if one thing isn't right or not ignored it'll show

        bool show = false;

        show |= (!EditorPrefs.HasKey(dismiss + buildTarget) && EditorUserBuildSettings.activeBuildTarget != myBuildTarget);

        show |= (!EditorPrefs.HasKey(dismiss + virRealitySupported) && PlayerSettings.virtualRealitySupported != virtualRealitySupported);

        show |= (!EditorPrefs.HasKey(dismiss + minimumAndroidSdk) && (int)PlayerSettings.Android.minSdkVersion != minAndroidSdk);

        show |= (!EditorPrefs.HasKey(dismiss + oculussdk) && !AssetDatabase.IsValidFolder("Assets/OVR"));

        show |= (!EditorPrefs.HasKey(dismiss + oculussig) && !AssetDatabase.IsValidFolder("Assets/Plugins/Android/assets"));
        

        if (show)
        { 
            window = GetWindow<GearVRSettings>(true);
            window.minSize = new Vector2(640, 480);
        }

        EditorApplication.update -= Update;
    }

    Vector2 scrollPosition;

    public void OnGUI()
    {
        //debug this to figure out why not showing


        EditorGUILayout.HelpBox("Recommended settings for Gear VR:", MessageType.Warning);

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        int numItems = 0;
        int numIgnoredItems = 0;
        
        //build target
        if (!EditorPrefs.HasKey(dismiss + buildTarget) &&
            EditorUserBuildSettings.activeBuildTarget != myBuildTarget)
        {
            ++numItems;

            GUILayout.Label(buildTarget);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button(string.Format(useSuggested, myBuildTarget)))
            {
#if (UNITY_5_3 || UNITY_5_4 || UNITY_5_5)
                EditorUserBuildSettings.SwitchActiveBuildTarget(myBuildTarget);
#else
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android,myBuildTarget);
#endif

            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Ignore"))
            {
                EditorPrefs.SetBool(dismiss + buildTarget, true);
            }

            GUILayout.EndHorizontal();
        }
        else if (EditorPrefs.HasKey(dismiss + buildTarget))
        {
            numIgnoredItems++;
        }

        //virtual reality supported
        if (!EditorPrefs.HasKey(dismiss + virRealitySupported) &&
            PlayerSettings.virtualRealitySupported != virtualRealitySupported)
        {
            ++numItems;

            GUILayout.Label(virRealitySupported);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button(string.Format(useSuggested, virtualRealitySupported)))
            {
                PlayerSettings.virtualRealitySupported = virtualRealitySupported;
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Ignore"))
            {
                EditorPrefs.SetBool(dismiss + virRealitySupported, true);
            }

            GUILayout.EndHorizontal();
        }
        else if (EditorPrefs.HasKey(dismiss + virRealitySupported))
        {
            numIgnoredItems++;
        }

        //minimum android sdk
        if (!EditorPrefs.HasKey(dismiss + minimumAndroidSdk) &&
            (int) PlayerSettings.Android.minSdkVersion != minAndroidSdk)
        {
            ++numItems;

            GUILayout.Label(minimumAndroidSdk);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button(string.Format(useSuggested, minimumAndroidSdk)))
            {
                PlayerSettings.Android.minSdkVersion = (AndroidSdkVersions) minAndroidSdk;
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Ignore"))
            {
                EditorPrefs.SetBool(dismiss + minimumAndroidSdk, true);
            }

            GUILayout.EndHorizontal();
        }
        else if (EditorPrefs.HasKey(dismiss + minimumAndroidSdk))
        {
            numIgnoredItems++;
        }

        //oculus utilities sdk
        if (!EditorPrefs.HasKey(dismiss + oculussdk) && !AssetDatabase.IsValidFolder("Assets/OVR"))
        {
            ++numItems;

            GUILayout.Label(oculussdk);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Download"))
            {
                Application.OpenURL("https://developer.oculus.com/downloads/package/oculus-utilities-for-unity-5/");
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Ignore"))
            {
                EditorPrefs.SetBool(dismiss + oculussdk, true);
            }

            GUILayout.EndHorizontal();
        }
        else if (EditorPrefs.HasKey(dismiss + oculussdk))
        {
            numIgnoredItems++;
        }

        //oculus signature file
        if (!EditorPrefs.HasKey(dismiss + oculussig) && !AssetDatabase.IsValidFolder("Assets/Plugins/Android/assets"))
        {
            ++numItems;

            GUILayout.Label(oculussig);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Instructions for creating your osig file"))
            {
                Application.OpenURL("https://dashboard.oculus.com/tools/osig-generator/");
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Ignore"))
            {
                EditorPrefs.SetBool(dismiss + oculussig, true);
            }

            GUILayout.EndHorizontal();
        }
        else if (EditorPrefs.HasKey(dismiss + oculussig))
        {
            numIgnoredItems++;
        }

        


        //rest of window logic
        if (numItems > 0)
            GUILayout.Space(50f);


        GUILayout.BeginHorizontal();
        


        if (GUILayout.Button("Clear Previously Ignored Recommendations (" + numIgnoredItems.ToString() + " ignored)"))
        {
            EditorPrefs.DeleteKey(dismiss + buildTarget);
            EditorPrefs.DeleteKey(dismiss + virRealitySupported);
            EditorPrefs.DeleteKey(dismiss + minimumAndroidSdk);
            EditorPrefs.DeleteKey(dismiss + oculussdk);
            EditorPrefs.DeleteKey(dismiss + oculussig);

        }

        GUILayout.EndHorizontal();

        GUILayout.EndScrollView();

        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();

        if (numItems > 0)
        {
            if (GUILayout.Button("Accept All Settings"))
            {
                // Only set those that have not been explicitly ignored.
                if (!EditorPrefs.HasKey(dismiss + buildTarget))
#if (UNITY_5_3 || UNITY_5_4 || UNITY_5_5)
                    EditorUserBuildSettings.SwitchActiveBuildTarget(myBuildTarget);
#else
                    EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android,myBuildTarget);
#endif
                if (!EditorPrefs.HasKey(dismiss + virRealitySupported))
                    PlayerSettings.virtualRealitySupported = virtualRealitySupported;
                if (!EditorPrefs.HasKey(dismiss + minimumAndroidSdk))
                    PlayerSettings.Android.minSdkVersion = (AndroidSdkVersions)minAndroidSdk;

              
                EditorUtility.DisplayDialog("Accept All", "Settings have been changed. If you haven't downloaded and imported the sdks or provided a signature file yet this is still required, depending on your platform.", "Ok");
            }

            if (GUILayout.Button("Ignore All"))
            {
                if (EditorUtility.DisplayDialog("Ignore Them All", "Are you sure?", "Yes", "Cancel"))
                {
                    // Only ignore those that do not currently match our recommended settings.

                    if (EditorUserBuildSettings.activeBuildTarget != myBuildTarget)
                        EditorPrefs.SetBool(dismiss + buildTarget, true);
                    if (PlayerSettings.virtualRealitySupported != virtualRealitySupported)
                        EditorPrefs.SetBool(dismiss + virRealitySupported, true);
                    if ((int)PlayerSettings.Android.minSdkVersion != minAndroidSdk)
                        EditorPrefs.SetBool(dismiss + minimumAndroidSdk, true);
                    if (!AssetDatabase.IsValidFolder("Assets/OVR"))
                        EditorPrefs.SetBool(dismiss + oculussdk, true);
                    if (!AssetDatabase.IsValidFolder("Assets/Plugins/Android/assets"))
                        EditorPrefs.SetBool(dismiss + oculussig, true);

                    Close();
                }
            }
        }
        else if (GUILayout.Button("Close"))
        {
            Close();
        }

        GUILayout.EndHorizontal();
    }
}

