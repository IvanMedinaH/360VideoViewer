using UnityEngine;
using System;
using System.Collections;
using EasyInputVR.Core;

namespace EasyInputVR.Misc
{

    [AddComponentMenu("EasyInputGearVR/Miscellaneous/Steering")]
    public class Steering : MonoBehaviour
    {

        Rigidbody myRigidbody;
        Vector3 myOrientation = Vector3.zero;
        bool gasPressed;
        bool brakePressed;
        float horizontal, vertical;
        float normalizeDegrees = 90f;
        float sensitivity = 10f;
        Vector3 actionVectorPosition;
        Vector3 computerVector;

        void OnEnable()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            EasyInputHelper.On_Motion += localMotion;
#endif
            EasyInputHelper.On_ClickStart += localClickStart;
            EasyInputHelper.On_ClickEnd += localClickEnd;

        }

        void OnDestroy()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            EasyInputHelper.On_Motion -= localMotion;
#endif
            EasyInputHelper.On_Click -= localClickStart;
            EasyInputHelper.On_Click -= localClickEnd;
        }

        void Start()
        {
            myRigidbody = this.GetComponent<Rigidbody>();
        }

        void Update()
        {
            //gas
            if (gasPressed)
            {
                if (myRigidbody.velocity.magnitude > 1f)
                    myRigidbody.AddForce(myRigidbody.velocity * sensitivity * .1f);
                else
                    myRigidbody.AddForce((this.transform.position - Camera.main.transform.position) * sensitivity * 5f);
            }

            //brake
            if (brakePressed)
            {
                if (myRigidbody.velocity.magnitude > 1f)
                    myRigidbody.AddForce(myRigidbody.velocity * -myRigidbody.velocity.magnitude);
                else
                    myRigidbody.AddForce(myRigidbody.velocity * -myRigidbody.velocity.magnitude * 10f);
            }

            //steering
            steerBall(myOrientation);
        }


        public void steerBall(Vector3 myOrientation)
        {


            if (myOrientation != Vector3.zero)
            {
                horizontal = myOrientation.z;
                vertical = myOrientation.x;

                //get into a -180 to 180 range
                horizontal = (horizontal > 180f) ? (horizontal - 360f) : horizontal;
                vertical = (vertical > 180f) ? (vertical - 360f) : vertical;

                horizontal = horizontal / normalizeDegrees;
                vertical = vertical / normalizeDegrees;

                horizontal *= -sensitivity;
                vertical *= sensitivity;
            }
            else
            {
                horizontal = 0f;
                vertical = 0f;
            }

            actionVectorPosition.x = horizontal;
            actionVectorPosition.y = 0f;
            actionVectorPosition.z = vertical;

            myRigidbody.AddForce(actionVectorPosition);

        }

        void localMotion(EasyInputVR.Core.Motion motion)
        {
            myOrientation = motion.currentOrientationEuler;
        }

        void localClickStart(ButtonClick button)
        {
            if (button.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTouchClick)
            {
                brakePressed = true;
            }
            else if (button.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTrigger)
            {
                gasPressed = true;
            }
        }

        void localClickEnd(ButtonClick button)
        {
            if (button.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTouchClick)
            {
                brakePressed = false;
            }
            else if (button.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTrigger)
            {
                gasPressed = false;
            }
        }

    }
}