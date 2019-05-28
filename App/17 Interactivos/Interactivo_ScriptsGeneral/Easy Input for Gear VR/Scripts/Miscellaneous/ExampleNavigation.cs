using UnityEngine;
using System.Collections;
using EasyInputVR.Core;
using UnityEngine.SceneManagement;

namespace EasyInputVR.Misc
{

    [AddComponentMenu("EasyInputGearVR/Miscellaneous/ExampleNavigation")]
    public class ExampleNavigation : MonoBehaviour
    {

        public void button1Submit()
        {
            SceneManager.LoadScene("tiltGearVRControllerExample");
        }

        public void button2Submit()
        {
            SceneManager.LoadScene("controlsExample");
        }

        public void button3Submit()
        {
            SceneManager.LoadScene("firstPersonGearVRController");
        }

        public void button4Submit()
        {
            SceneManager.LoadScene("GuiNavigationExample");
        }

        public void button5Submit()
        {
            SceneManager.LoadScene("GamepadControllerDiagnosticExample");
        }

        public void button6Submit()
        {
            SceneManager.LoadScene("GVRcontrollerDiagnosticExample");
        }

        public void button7Submit()
        {
            SceneManager.LoadScene("bowlingExample");
        }

        public void button8Submit()
        {
            SceneManager.LoadScene("pointerExample");
        }

    }
}