using UnityEngine;
using System.Collections;
using EasyInputVR.Core;
using System;

namespace EasyInputVR.StandardControllers
{

    [AddComponentMenu("EasyInputGearVR/Standard Controllers/StandardAccelerometerController")]
    public class StandardTiltController : MonoBehaviour
    {
        public EasyInputConstants.AXIS tiltHorizontal = EasyInputConstants.AXIS.XAxis;
        public EasyInputConstants.AXIS tiltVertical = EasyInputConstants.AXIS.YAxis;
        public EasyInputConstants.ACTION_TYPE action = EasyInputConstants.ACTION_TYPE.Position;
        public float sensitivity = 1f;

        //runtime variables
        Vector3 actionVector3;
        float horizontal;
        float vertical;
        float normalizeDegrees = 90f;


        void OnEnable()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            EasyInputHelper.On_Motion += localMotion;
#endif

        }

        void OnDestroy()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            EasyInputHelper.On_Motion -= localMotion;
#endif
        }

        // Update is called once per frame
        void Update()
        {
        }

        void localMotion(EasyInputVR.Core.Motion motion)
        {

            //accelerometers due to gravity can really only sense 2 axis (can't filter out gravity)
            //here we convert those 2 axis into horizontal and vertical and normalize

            horizontal = motion.currentOrientationEuler.z;
            vertical = motion.currentOrientationEuler.x;

            //get into a -180 to 180 range
            horizontal = (horizontal > 180f) ? (horizontal - 360f) : horizontal;
            vertical = (vertical > 180f) ? (vertical - 360f) : vertical;

            horizontal = horizontal / normalizeDegrees;
            vertical = vertical / normalizeDegrees;

            horizontal *= -sensitivity * Time.deltaTime * 100f;
            vertical *= -sensitivity * Time.deltaTime * 100f;

            actionVector3 = EasyInputUtilities.getControllerVector3(horizontal, vertical, tiltHorizontal, tiltVertical);

            switch (action)
            {
                case EasyInputConstants.ACTION_TYPE.Position:
                    transform.position += actionVector3;
                    break;
                case EasyInputConstants.ACTION_TYPE.Rotation:
                    transform.Rotate(actionVector3, Space.World);
                    break;
                case EasyInputConstants.ACTION_TYPE.LocalPosition:
                    transform.Translate(actionVector3);
                    break;
                case EasyInputConstants.ACTION_TYPE.LocalRotation:
                    transform.Rotate(actionVector3);
                    break;
                case EasyInputConstants.ACTION_TYPE.LocalScale:
                    transform.localScale += actionVector3;
                    break;
                default:
                    Debug.Log("Invalid Action");
                    break;

            }
        }


    }

}

