using UnityEngine;
using System;
using System.Collections;
using EasyInputVR.Core;
using UnityEngine.SceneManagement;

namespace EasyInputVR.Misc
{

    [AddComponentMenu("EasyInputGearVR/Miscellaneous/ExitToMaster")]
    public class ExitToMaster : MonoBehaviour
    {

        void OnEnable()
        {
            EasyInputHelper.On_ClickStart += localClickStart;
        }

        void OnDestroy()
        {
            EasyInputHelper.On_Click -= localClickStart;
        }

        void localClickStart(ButtonClick button)
        {
            if (EasyInputHelper.isGearVR || Application.isEditor)
            {
                if (button.button == EasyInputConstants.CONTROLLER_BUTTON.Back)
                {
                    SceneManager.LoadScene("MasterExample");
                }
            }
        }

    }
}