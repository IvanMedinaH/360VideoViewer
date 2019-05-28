using UnityEngine;
using System.Collections;
using EasyInputVR.Core;
using System;

namespace EasyInputVR.Misc
{

    [AddComponentMenu("EasyInputGearVR/Miscellaneous/pointerSwitch")]
    public class pointerSwitch : MonoBehaviour
    {

        public GameObject laserPointer;
        public GameObject gazePointer;
        public GameObject curvedPointer;

        public void switchToLaser()
        {

            gazePointer.SetActive(false);
            curvedPointer.SetActive(false);
            laserPointer.SetActive(true);
            StartCoroutine("setInitialScaleLaser", .1f);
        }

        public void switchToGaze()
        {
            laserPointer.SetActive(false);
            curvedPointer.SetActive(false);
            gazePointer.SetActive(true);
            StartCoroutine("setInitialScaleGaze", .1f);
        }

        public void switchToCurved()
        {

            gazePointer.SetActive(false);
            laserPointer.SetActive(false);
            curvedPointer.SetActive(true);
            StartCoroutine("setInitialScaleCurved", .1f);
        }

        IEnumerator setInitialScaleGaze(float delay)
        {
            yield return new WaitForSeconds(delay);
            gazePointer.GetComponent<StandardControllers.StandardGazePointer>().setInitialScale(new Vector3(.4f, .4f, .4f));
        }

        IEnumerator setInitialScaleLaser(float delay)
        {
            yield return new WaitForSeconds(delay);
            laserPointer.GetComponent<StandardControllers.StandardLaserPointer>().setInitialScale(new Vector3(.2f, .2f, .2f));
        }

        IEnumerator setInitialScaleCurved(float delay)
        {
            yield return new WaitForSeconds(delay);
            curvedPointer.GetComponent<StandardControllers.StandardCurvedLaserPointer>().setInitialScale(new Vector3(2f, 2f, 2f));
        }



    }
}
