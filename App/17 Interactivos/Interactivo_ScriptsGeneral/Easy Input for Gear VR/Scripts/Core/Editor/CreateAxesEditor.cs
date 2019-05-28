using UnityEngine;
using UnityEditor;
using System.Collections;
using EasyInputVR.Core;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class CreateAxesEditor
{

    static CreateAxesEditor()
    {
        EditorApplication.hierarchyWindowChanged += OnHierarchyChange;
        EditorApplication.hierarchyWindowChanged += addOtherExamplesToBuildSettings;
    }

    static void addOtherExamplesToBuildSettings()
    {
        //if we are in the master example scene add all the scenes to the build settings
        if (EditorSceneManager.GetActiveScene().name.Contains("MasterExample"))
        {
            var temp = EditorBuildSettings.scenes;

            if ((temp != null && temp.Length > 0))
            {
                if (temp[0].path.Equals("Assets/Easy Input for Gear VR/Scenes/MasterExample.unity"))
                {
                    //we already have our scenes in
                    EditorApplication.hierarchyWindowChanged -= addOtherExamplesToBuildSettings;
                    return;
                }
            }

            EditorBuildSettingsScene[] newBuildSettings = new EditorBuildSettingsScene[temp.Length + 9];
            newBuildSettings[0] = new EditorBuildSettingsScene("Assets/Easy Input for Gear VR/Scenes/MasterExample.unity", true);
            newBuildSettings[1] = new EditorBuildSettingsScene("Assets/Easy Input for Gear VR/Scenes/SpecificExamples/tiltGearVRControllerExample.unity", true);
            newBuildSettings[2] = new EditorBuildSettingsScene("Assets/Easy Input for Gear VR/Scenes/SpecificExamples/controlsExample.unity", true);
            newBuildSettings[3] = new EditorBuildSettingsScene("Assets/Easy Input for Gear VR/Scenes/SpecificExamples/firstPersonGearVRController.unity", true);
            newBuildSettings[4] = new EditorBuildSettingsScene("Assets/Easy Input for Gear VR/Scenes/SpecificExamples/GuiNavigationExample.unity", true);
            newBuildSettings[5] = new EditorBuildSettingsScene("Assets/Easy Input for Gear VR/Scenes/SpecificExamples/GamepadControllerDiagnosticExample.unity", true);
            newBuildSettings[6] = new EditorBuildSettingsScene("Assets/Easy Input for Gear VR/Scenes/SpecificExamples/GVRcontrollerDiagnosticExample.unity", true);
            newBuildSettings[7] = new EditorBuildSettingsScene("Assets/Easy Input for Gear VR/Scenes/SpecificExamples/bowlingExample.unity", true);
            newBuildSettings[8] = new EditorBuildSettingsScene("Assets/Easy Input for Gear VR/Scenes/SpecificExamples/pointerExample.unity", true);

            //put the old ones at the end
            for (int i = 0;i< temp.Length; i++)
            {
                newBuildSettings[i + 9] = temp[i];
            }


            EditorBuildSettings.scenes = newBuildSettings;

            //we only need to execute once           
            EditorApplication.hierarchyWindowChanged -= addOtherExamplesToBuildSettings;
        }
    }
    static void OnHierarchyChange()
    {
        //add our axes (if not already present when we read the file below)
        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");
        SerializedProperty childElement;

        bool isAxesPresent = false;

        int count = axesProperty.arraySize;
            

        for (int i = 0; i < count; i++)
        {
            childElement = axesProperty.GetArrayElementAtIndex(i);
            if (GetChildProperty(childElement, "m_Name").stringValue.Equals(EasyInputConstants.P1_LEFTSTICK_HORIZONTAL))
            {
                isAxesPresent = true;
                break;
            }
        }

        //the axes were not present add them and save the file (there are 24 axes 8 per player and 3 players)
        //also 8 keyboard emulation axes and 6 for the remote touches for 38 total
        //unfortunately have to add manually
        if (!isAxesPresent)
        {
            axesProperty.arraySize += 21;
            serializedObject.ApplyModifiedProperties();


            //player 1 (keyboard emulation)
            //EIVR_LeftStick_Horizontal
            childElement = axesProperty.GetArrayElementAtIndex(count);

            GetChildProperty(childElement, "m_Name").stringValue = EasyInputConstants.P1_LEFTSTICK_HORIZONTAL;
            GetChildProperty(childElement, "descriptiveName").stringValue = "";
            GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
            GetChildProperty(childElement, "negativeButton").stringValue = "left";
            GetChildProperty(childElement, "positiveButton").stringValue = "right";
            GetChildProperty(childElement, "altNegativeButton").stringValue = "";
            GetChildProperty(childElement, "altPositiveButton").stringValue = "";
            GetChildProperty(childElement, "gravity").floatValue = 1000f;
            GetChildProperty(childElement, "dead").floatValue = .001f;
            GetChildProperty(childElement, "sensitivity").floatValue = 1000f;
            GetChildProperty(childElement, "snap").boolValue = false;
            GetChildProperty(childElement, "invert").boolValue = false;
            GetChildProperty(childElement, "type").intValue = 0;
            GetChildProperty(childElement, "axis").intValue = 0;
            GetChildProperty(childElement, "joyNum").intValue = 1;
            count++;

            //EIVR_LeftStick_Vertical
            childElement = axesProperty.GetArrayElementAtIndex(count);

            GetChildProperty(childElement, "m_Name").stringValue = EasyInputConstants.P1_LEFTSTICK_VERTICAL;
            GetChildProperty(childElement, "descriptiveName").stringValue = "";
            GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
            GetChildProperty(childElement, "negativeButton").stringValue = "down";
            GetChildProperty(childElement, "positiveButton").stringValue = "up";
            GetChildProperty(childElement, "altNegativeButton").stringValue = "";
            GetChildProperty(childElement, "altPositiveButton").stringValue = "";
            GetChildProperty(childElement, "gravity").floatValue = 1000f;
            GetChildProperty(childElement, "dead").floatValue = .001f;
            GetChildProperty(childElement, "sensitivity").floatValue = 1000f;
            GetChildProperty(childElement, "snap").boolValue = false;
            GetChildProperty(childElement, "invert").boolValue = false;
            GetChildProperty(childElement, "type").intValue = 0;
            GetChildProperty(childElement, "axis").intValue = 1;
            GetChildProperty(childElement, "joyNum").intValue = 1;
            count++;

            //EIVR_RightStick_Horizontal
            childElement = axesProperty.GetArrayElementAtIndex(count);

            GetChildProperty(childElement, "m_Name").stringValue = EasyInputConstants.P1_RIGHTSTICK_HORIZONTAL;
            GetChildProperty(childElement, "descriptiveName").stringValue = "";
            GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
            GetChildProperty(childElement, "negativeButton").stringValue = "[4]";
            GetChildProperty(childElement, "positiveButton").stringValue = "[6]";
            GetChildProperty(childElement, "altNegativeButton").stringValue = "";
            GetChildProperty(childElement, "altPositiveButton").stringValue = "";
            GetChildProperty(childElement, "gravity").floatValue = 1000f;
            GetChildProperty(childElement, "dead").floatValue = .001f;
            GetChildProperty(childElement, "sensitivity").floatValue = 1000f;
            GetChildProperty(childElement, "snap").boolValue = false;
            GetChildProperty(childElement, "invert").boolValue = false;
            GetChildProperty(childElement, "type").intValue = 0;
            GetChildProperty(childElement, "axis").intValue = 2;
            GetChildProperty(childElement, "joyNum").intValue = 1;
            count++;

            //EIVR_RightStick_Vertical
            childElement = axesProperty.GetArrayElementAtIndex(count);

            GetChildProperty(childElement, "m_Name").stringValue = EasyInputConstants.P1_RIGHTSTICK_VERTICAL;
            GetChildProperty(childElement, "descriptiveName").stringValue = "";
            GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
            GetChildProperty(childElement, "negativeButton").stringValue = "[2]";
            GetChildProperty(childElement, "positiveButton").stringValue = "[8]";
            GetChildProperty(childElement, "altNegativeButton").stringValue = "";
            GetChildProperty(childElement, "altPositiveButton").stringValue = "";
            GetChildProperty(childElement, "gravity").floatValue = 1000f;
            GetChildProperty(childElement, "dead").floatValue = .001f;
            GetChildProperty(childElement, "sensitivity").floatValue = 1000f;
            GetChildProperty(childElement, "snap").boolValue = false;
            GetChildProperty(childElement, "invert").boolValue = false;
            GetChildProperty(childElement, "type").intValue = 0;
            GetChildProperty(childElement, "axis").intValue = 3;
            GetChildProperty(childElement, "joyNum").intValue = 1;
            count++;

            //EIVR_Dpad_Horizontal
            childElement = axesProperty.GetArrayElementAtIndex(count);

            GetChildProperty(childElement, "m_Name").stringValue = EasyInputConstants.P1_DPAD_HORIZONTAL;
            GetChildProperty(childElement, "descriptiveName").stringValue = "";
            GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
            GetChildProperty(childElement, "negativeButton").stringValue = "delete";
            GetChildProperty(childElement, "positiveButton").stringValue = "page down";
            GetChildProperty(childElement, "altNegativeButton").stringValue = "";
            GetChildProperty(childElement, "altPositiveButton").stringValue = "";
            GetChildProperty(childElement, "gravity").floatValue = 1000f;
            GetChildProperty(childElement, "dead").floatValue = .001f;
            GetChildProperty(childElement, "sensitivity").floatValue = 1000f;
            GetChildProperty(childElement, "snap").boolValue = false;
            GetChildProperty(childElement, "invert").boolValue = false;
            GetChildProperty(childElement, "type").intValue = 0;
            GetChildProperty(childElement, "axis").intValue = 0;
            GetChildProperty(childElement, "joyNum").intValue = 1;
            count++;

            //EIVR_Dpad_Vertical
            childElement = axesProperty.GetArrayElementAtIndex(count);

            GetChildProperty(childElement, "m_Name").stringValue = EasyInputConstants.P1_DPAD_VERTICAL;
            GetChildProperty(childElement, "descriptiveName").stringValue = "";
            GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
            GetChildProperty(childElement, "negativeButton").stringValue = "end";
            GetChildProperty(childElement, "positiveButton").stringValue = "home";
            GetChildProperty(childElement, "altNegativeButton").stringValue = "";
            GetChildProperty(childElement, "altPositiveButton").stringValue = "";
            GetChildProperty(childElement, "gravity").floatValue = 1000f;
            GetChildProperty(childElement, "dead").floatValue = .001f;
            GetChildProperty(childElement, "sensitivity").floatValue = 1000f;
            GetChildProperty(childElement, "snap").boolValue = false;
            GetChildProperty(childElement, "invert").boolValue = false;
            GetChildProperty(childElement, "type").intValue = 0;
            GetChildProperty(childElement, "axis").intValue = 0;
            GetChildProperty(childElement, "joyNum").intValue = 1;
            count++;

            //EIVR_LeftTrigger
            childElement = axesProperty.GetArrayElementAtIndex(count);

            GetChildProperty(childElement, "m_Name").stringValue = EasyInputConstants.P1_LEFTTRIGGER;
            GetChildProperty(childElement, "descriptiveName").stringValue = "";
            GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
            GetChildProperty(childElement, "negativeButton").stringValue = "[0]";
            GetChildProperty(childElement, "positiveButton").stringValue = "";
            GetChildProperty(childElement, "altNegativeButton").stringValue = "";
            GetChildProperty(childElement, "altPositiveButton").stringValue = "";
            GetChildProperty(childElement, "gravity").floatValue = 1000f;
            GetChildProperty(childElement, "dead").floatValue = .001f;
            GetChildProperty(childElement, "sensitivity").floatValue = 1000f;
            GetChildProperty(childElement, "snap").boolValue = false;
            GetChildProperty(childElement, "invert").boolValue = false;
            GetChildProperty(childElement, "type").intValue = 0;
            GetChildProperty(childElement, "axis").intValue = 0;
            GetChildProperty(childElement, "joyNum").intValue = 1;
            count++;

            //EIVR_RightTrigger
            childElement = axesProperty.GetArrayElementAtIndex(count);

            GetChildProperty(childElement, "m_Name").stringValue = EasyInputConstants.P1_RIGHTTRIGGER;
            GetChildProperty(childElement, "descriptiveName").stringValue = "";
            GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
            GetChildProperty(childElement, "negativeButton").stringValue = "[.]";
            GetChildProperty(childElement, "positiveButton").stringValue = "";
            GetChildProperty(childElement, "altNegativeButton").stringValue = "";
            GetChildProperty(childElement, "altPositiveButton").stringValue = "";
            GetChildProperty(childElement, "gravity").floatValue = 1000f;
            GetChildProperty(childElement, "dead").floatValue = .001f;
            GetChildProperty(childElement, "sensitivity").floatValue = 1000f;
            GetChildProperty(childElement, "snap").boolValue = false;
            GetChildProperty(childElement, "invert").boolValue = false;
            GetChildProperty(childElement, "type").intValue = 0;
            GetChildProperty(childElement, "axis").intValue = 0;
            GetChildProperty(childElement, "joyNum").intValue = 1;
            count++;

            //player 1 (real)
            //EIVR_LeftStick_Horizontal
            childElement = axesProperty.GetArrayElementAtIndex(count);

            GetChildProperty(childElement, "m_Name").stringValue = EasyInputConstants.P1_LEFTSTICK_HORIZONTAL;
            GetChildProperty(childElement, "descriptiveName").stringValue = "";
            GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
            GetChildProperty(childElement, "negativeButton").stringValue = "";
            GetChildProperty(childElement, "positiveButton").stringValue = "";
            GetChildProperty(childElement, "altNegativeButton").stringValue = "";
            GetChildProperty(childElement, "altPositiveButton").stringValue = "";
            GetChildProperty(childElement, "gravity").floatValue = 0f;
            GetChildProperty(childElement, "dead").floatValue = .19f;
            GetChildProperty(childElement, "sensitivity").floatValue = 1f;
            GetChildProperty(childElement, "snap").boolValue = false;
            GetChildProperty(childElement, "invert").boolValue = false;
            GetChildProperty(childElement, "type").intValue = 2;
            GetChildProperty(childElement, "axis").intValue = 0;
            GetChildProperty(childElement, "joyNum").intValue = 1;
            count++;

            //EIVR_LeftStick_Vertical
            childElement = axesProperty.GetArrayElementAtIndex(count);

            GetChildProperty(childElement, "m_Name").stringValue = EasyInputConstants.P1_LEFTSTICK_VERTICAL;
            GetChildProperty(childElement, "descriptiveName").stringValue = "";
            GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
            GetChildProperty(childElement, "negativeButton").stringValue = "";
            GetChildProperty(childElement, "positiveButton").stringValue = "";
            GetChildProperty(childElement, "altNegativeButton").stringValue = "";
            GetChildProperty(childElement, "altPositiveButton").stringValue = "";
            GetChildProperty(childElement, "gravity").floatValue = 0f;
            GetChildProperty(childElement, "dead").floatValue = .19f;
            GetChildProperty(childElement, "sensitivity").floatValue = 1f;
            GetChildProperty(childElement, "snap").boolValue = false;
            GetChildProperty(childElement, "invert").boolValue = true;
            GetChildProperty(childElement, "type").intValue = 2;
            GetChildProperty(childElement, "axis").intValue = 1;
            GetChildProperty(childElement, "joyNum").intValue = 1;
            count++;

            //EIVR_RightStick_Horizontal
            childElement = axesProperty.GetArrayElementAtIndex(count);

            GetChildProperty(childElement, "m_Name").stringValue = EasyInputConstants.P1_RIGHTSTICK_HORIZONTAL;
            GetChildProperty(childElement, "descriptiveName").stringValue = "";
            GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
            GetChildProperty(childElement, "negativeButton").stringValue = "";
            GetChildProperty(childElement, "positiveButton").stringValue = "";
            GetChildProperty(childElement, "altNegativeButton").stringValue = "";
            GetChildProperty(childElement, "altPositiveButton").stringValue = "";
            GetChildProperty(childElement, "gravity").floatValue = 0f;
            GetChildProperty(childElement, "dead").floatValue = .19f;
            GetChildProperty(childElement, "sensitivity").floatValue = 1f;
            GetChildProperty(childElement, "snap").boolValue = false;
            GetChildProperty(childElement, "invert").boolValue = false;
            GetChildProperty(childElement, "type").intValue = 2;
            GetChildProperty(childElement, "axis").intValue = 2;
            GetChildProperty(childElement, "joyNum").intValue = 1;
            count++;

            //EIVR_RightStick_Vertical
            childElement = axesProperty.GetArrayElementAtIndex(count);

            GetChildProperty(childElement, "m_Name").stringValue = EasyInputConstants.P1_RIGHTSTICK_VERTICAL;
            GetChildProperty(childElement, "descriptiveName").stringValue = "";
            GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
            GetChildProperty(childElement, "negativeButton").stringValue = "";
            GetChildProperty(childElement, "positiveButton").stringValue = "";
            GetChildProperty(childElement, "altNegativeButton").stringValue = "";
            GetChildProperty(childElement, "altPositiveButton").stringValue = "";
            GetChildProperty(childElement, "gravity").floatValue = 0f;
            GetChildProperty(childElement, "dead").floatValue = .19f;
            GetChildProperty(childElement, "sensitivity").floatValue = 1f;
            GetChildProperty(childElement, "snap").boolValue = false;
            GetChildProperty(childElement, "invert").boolValue = true;
            GetChildProperty(childElement, "type").intValue = 2;
            GetChildProperty(childElement, "axis").intValue = 3;
            GetChildProperty(childElement, "joyNum").intValue = 1;
            count++;

            //EIVR_Dpad_Horizontal
            childElement = axesProperty.GetArrayElementAtIndex(count);

            GetChildProperty(childElement, "m_Name").stringValue = EasyInputConstants.P1_DPAD_HORIZONTAL;
            GetChildProperty(childElement, "descriptiveName").stringValue = "";
            GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
            GetChildProperty(childElement, "negativeButton").stringValue = "";
            GetChildProperty(childElement, "positiveButton").stringValue = "";
            GetChildProperty(childElement, "altNegativeButton").stringValue = "";
            GetChildProperty(childElement, "altPositiveButton").stringValue = "";
            GetChildProperty(childElement, "gravity").floatValue = 1000f;
            GetChildProperty(childElement, "dead").floatValue = .001f;
            GetChildProperty(childElement, "sensitivity").floatValue = 1000f;
            GetChildProperty(childElement, "snap").boolValue = false;
            GetChildProperty(childElement, "invert").boolValue = false;
            GetChildProperty(childElement, "type").intValue = 2;
            GetChildProperty(childElement, "axis").intValue = 4;
            GetChildProperty(childElement, "joyNum").intValue = 1;
            count++;

            //EIVR_Dpad_Vertical
            childElement = axesProperty.GetArrayElementAtIndex(count);

            GetChildProperty(childElement, "m_Name").stringValue = EasyInputConstants.P1_DPAD_VERTICAL;
            GetChildProperty(childElement, "descriptiveName").stringValue = "";
            GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
            GetChildProperty(childElement, "negativeButton").stringValue = "joystick 1 button 6";
            GetChildProperty(childElement, "positiveButton").stringValue = "joystick 1 button 4";
            GetChildProperty(childElement, "altNegativeButton").stringValue = "";
            GetChildProperty(childElement, "altPositiveButton").stringValue = "";
            GetChildProperty(childElement, "gravity").floatValue = 1000f;
            GetChildProperty(childElement, "dead").floatValue = .001f;
            GetChildProperty(childElement, "sensitivity").floatValue = 1000f;
            GetChildProperty(childElement, "snap").boolValue = false;
            GetChildProperty(childElement, "invert").boolValue = false;
            GetChildProperty(childElement, "type").intValue = 2;
            GetChildProperty(childElement, "axis").intValue = 5;
            GetChildProperty(childElement, "joyNum").intValue = 1;
            count++;

            //EIVR_LeftTrigger
            childElement = axesProperty.GetArrayElementAtIndex(count);

            GetChildProperty(childElement, "m_Name").stringValue = EasyInputConstants.P1_LEFTTRIGGER;
            GetChildProperty(childElement, "descriptiveName").stringValue = "";
            GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
            GetChildProperty(childElement, "negativeButton").stringValue = "";
            GetChildProperty(childElement, "positiveButton").stringValue = "";
            GetChildProperty(childElement, "altNegativeButton").stringValue = "";
            GetChildProperty(childElement, "altPositiveButton").stringValue = "";
            GetChildProperty(childElement, "gravity").floatValue = 0f;
            GetChildProperty(childElement, "dead").floatValue = .019f;
            GetChildProperty(childElement, "sensitivity").floatValue = 1f;
            GetChildProperty(childElement, "snap").boolValue = false;
            GetChildProperty(childElement, "invert").boolValue = false;
            GetChildProperty(childElement, "type").intValue = 2;
            GetChildProperty(childElement, "axis").intValue = 6;
            GetChildProperty(childElement, "joyNum").intValue = 1;
            count++;

            //EIVR_LeftTrigger
            childElement = axesProperty.GetArrayElementAtIndex(count);

            GetChildProperty(childElement, "m_Name").stringValue = EasyInputConstants.P1_LEFTTRIGGER;
            GetChildProperty(childElement, "descriptiveName").stringValue = "";
            GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
            GetChildProperty(childElement, "negativeButton").stringValue = "";
            GetChildProperty(childElement, "positiveButton").stringValue = "";
            GetChildProperty(childElement, "altNegativeButton").stringValue = "";
            GetChildProperty(childElement, "altPositiveButton").stringValue = "";
            GetChildProperty(childElement, "gravity").floatValue = 0f;
            GetChildProperty(childElement, "dead").floatValue = .019f;
            GetChildProperty(childElement, "sensitivity").floatValue = 1f;
            GetChildProperty(childElement, "snap").boolValue = false;
            GetChildProperty(childElement, "invert").boolValue = false;
            GetChildProperty(childElement, "type").intValue = 2;
            GetChildProperty(childElement, "axis").intValue = 12;
            GetChildProperty(childElement, "joyNum").intValue = 1;
            count++;

            //EIVR_LeftTrigger
            childElement = axesProperty.GetArrayElementAtIndex(count);

            GetChildProperty(childElement, "m_Name").stringValue = EasyInputConstants.P1_LEFTTRIGGER;
            GetChildProperty(childElement, "descriptiveName").stringValue = "";
            GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
            GetChildProperty(childElement, "negativeButton").stringValue = "";
            GetChildProperty(childElement, "positiveButton").stringValue = "";
            GetChildProperty(childElement, "altNegativeButton").stringValue = "";
            GetChildProperty(childElement, "altPositiveButton").stringValue = "";
            GetChildProperty(childElement, "gravity").floatValue = 0f;
            GetChildProperty(childElement, "dead").floatValue = .019f;
            GetChildProperty(childElement, "sensitivity").floatValue = 1f;
            GetChildProperty(childElement, "snap").boolValue = false;
            GetChildProperty(childElement, "invert").boolValue = false;
            GetChildProperty(childElement, "type").intValue = 2;
            GetChildProperty(childElement, "axis").intValue = 13;
            GetChildProperty(childElement, "joyNum").intValue = 1;
            count++;

            //EIVR_LeftTrigger
            childElement = axesProperty.GetArrayElementAtIndex(count);

            GetChildProperty(childElement, "m_Name").stringValue = EasyInputConstants.P1_LEFTTRIGGER;
            GetChildProperty(childElement, "descriptiveName").stringValue = "";
            GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
            GetChildProperty(childElement, "negativeButton").stringValue = "";
            GetChildProperty(childElement, "positiveButton").stringValue = "joystick 1 button 6";
            GetChildProperty(childElement, "altNegativeButton").stringValue = "";
            GetChildProperty(childElement, "altPositiveButton").stringValue = "";
            GetChildProperty(childElement, "gravity").floatValue = 1000f;
            GetChildProperty(childElement, "dead").floatValue = .001f;
            GetChildProperty(childElement, "sensitivity").floatValue = 1000f;
            GetChildProperty(childElement, "snap").boolValue = false;
            GetChildProperty(childElement, "invert").boolValue = false;
            GetChildProperty(childElement, "type").intValue = 0;
            GetChildProperty(childElement, "axis").intValue = 0;
            GetChildProperty(childElement, "joyNum").intValue = 1;
            count++;

            //EIVR_RightTrigger
            childElement = axesProperty.GetArrayElementAtIndex(count);

            GetChildProperty(childElement, "m_Name").stringValue = EasyInputConstants.P1_RIGHTTRIGGER;
            GetChildProperty(childElement, "descriptiveName").stringValue = "";
            GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
            GetChildProperty(childElement, "negativeButton").stringValue = "";
            GetChildProperty(childElement, "positiveButton").stringValue = "";
            GetChildProperty(childElement, "altNegativeButton").stringValue = "";
            GetChildProperty(childElement, "altPositiveButton").stringValue = "";
            GetChildProperty(childElement, "gravity").floatValue = 0f;
            GetChildProperty(childElement, "dead").floatValue = .019f;
            GetChildProperty(childElement, "sensitivity").floatValue = 1f;
            GetChildProperty(childElement, "snap").boolValue = false;
            GetChildProperty(childElement, "invert").boolValue = false;
            GetChildProperty(childElement, "type").intValue = 2;
            GetChildProperty(childElement, "axis").intValue = 7;
            GetChildProperty(childElement, "joyNum").intValue = 1;
            count++;

            //EIVR_RightTrigger
            childElement = axesProperty.GetArrayElementAtIndex(count);

            GetChildProperty(childElement, "m_Name").stringValue = EasyInputConstants.P1_RIGHTTRIGGER;
            GetChildProperty(childElement, "descriptiveName").stringValue = "";
            GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
            GetChildProperty(childElement, "negativeButton").stringValue = "";
            GetChildProperty(childElement, "positiveButton").stringValue = "";
            GetChildProperty(childElement, "altNegativeButton").stringValue = "";
            GetChildProperty(childElement, "altPositiveButton").stringValue = "";
            GetChildProperty(childElement, "gravity").floatValue = 0f;
            GetChildProperty(childElement, "dead").floatValue = .019f;
            GetChildProperty(childElement, "sensitivity").floatValue = 1f;
            GetChildProperty(childElement, "snap").boolValue = false;
            GetChildProperty(childElement, "invert").boolValue = false;
            GetChildProperty(childElement, "type").intValue = 2;
            GetChildProperty(childElement, "axis").intValue = 11;
            GetChildProperty(childElement, "joyNum").intValue = 1;
            count++;

            //EIVR_RightTrigger
            childElement = axesProperty.GetArrayElementAtIndex(count);

            GetChildProperty(childElement, "m_Name").stringValue = EasyInputConstants.P1_RIGHTTRIGGER;
            GetChildProperty(childElement, "descriptiveName").stringValue = "";
            GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
            GetChildProperty(childElement, "negativeButton").stringValue = "";
            GetChildProperty(childElement, "positiveButton").stringValue = "joystick 1 button 7";
            GetChildProperty(childElement, "altNegativeButton").stringValue = "";
            GetChildProperty(childElement, "altPositiveButton").stringValue = "";
            GetChildProperty(childElement, "gravity").floatValue = 1000f;
            GetChildProperty(childElement, "dead").floatValue = .001f;
            GetChildProperty(childElement, "sensitivity").floatValue = 1000f;
            GetChildProperty(childElement, "snap").boolValue = false;
            GetChildProperty(childElement, "invert").boolValue = false;
            GetChildProperty(childElement, "type").intValue = 0;
            GetChildProperty(childElement, "axis").intValue = 0;
            GetChildProperty(childElement, "joyNum").intValue = 1;

            serializedObject.ApplyModifiedProperties();
        }


        //we only need to execute once           
        EditorApplication.hierarchyWindowChanged -= OnHierarchyChange;
        

    }

    public static SerializedProperty GetChildProperty(SerializedProperty parent, string name)
    {

        //copy so we don't iterate original
        SerializedProperty copiedProperty = parent.Copy();

        bool moreChildren = true;

        //step one level into child
        copiedProperty.Next(true);

        //iterate on all properties one level deep
        while (moreChildren)
        {
            //found the child we were looking for
            if (copiedProperty.name.Equals(name))
                return copiedProperty;

            //move to the next property
            moreChildren = copiedProperty.Next(false);
        }

        //if we get here we didn't find it
        return null;
    }

}