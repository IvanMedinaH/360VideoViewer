using UnityEngine;
using System.Collections;
using EasyInputVR.Core;
using System;

namespace EasyInputVR.Misc
{

    [AddComponentMenu("EasyInputGearVR/Miscellaneous/InstanceController")]
    public class InstanceController : MonoBehaviour
    {
        string exc;

        // Use this for initialization
        void Start()
        {
            //check for and try and replace with controller prefabs
            GameObject temp = null;

            Vector3 originalScale = this.transform.localScale;
            this.transform.localScale = new Vector3(1f, 1f, 1f);
#if UNITY_5_3
            try
            {
                temp = Instantiate(Resources.Load("gearvrcontroller"), this.transform.position, this.transform.rotation) as GameObject;
            }
            catch (Exception Ex)
            {
                exc = Ex.Message;
                exc = exc.ToString();
            }
                if (temp != null)
                    temp.transform.parent = this.transform;
#endif
#if !UNITY_5_3
            try
            {
                temp = Instantiate(Resources.Load("gearvrcontroller"), this.transform) as GameObject;
            }
            catch (Exception Ex)
            {
                exc = Ex.Message;
                exc = exc.ToString();
            }
#endif


            if (temp != null)
            {
                temp.transform.localScale = new Vector3(1f, 1f, 1f);
                temp.transform.localRotation = Quaternion.identity;
                temp.transform.localPosition = Vector3.zero;
                this.GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                this.transform.localScale = originalScale;
            }

        }
    }
}
