using UnityEngine;
using System.Collections;
using EasyInputVR.Core;
using System;

namespace EasyInputVR.StandardControllers
{

    [AddComponentMenu("EasyInputGearVR/Standard Controllers/StandardLookDpadController")]
    public class StandardLookDpadController : StandardBaseMovement
    {
        [Range(0f, 1f)]
        public float deadZone = .2f;
        public float sensitivity = 1f;
        public EasyInputConstants.DPAD_MODE dpadMode = EasyInputConstants.DPAD_MODE.RegisterAlways;
        public GameObject lookObject;

        //runtime variables
        bool blockInput = false;
        Vector3 actionVector3;
        float horizontal;
        float vertical;
        bool isClicking = false;
        Vector3 previousPos;


        void OnEnable()
        {
            EasyInputHelper.On_Touch += localAxis;

            if (dpadMode == EasyInputConstants.DPAD_MODE.RegisterOnlyWhenNotClicking)
            {
                EasyInputHelper.On_ClickStart += localClickStart;
                EasyInputHelper.On_ClickEnd += localClickEnd;
            }
        }

        void OnDestroy()
        {
            EasyInputHelper.On_Touch -= localAxis;

            if (dpadMode == EasyInputConstants.DPAD_MODE.RegisterOnlyWhenNotClicking)
            {
                EasyInputHelper.On_ClickStart -= localClickStart;
                EasyInputHelper.On_ClickEnd -= localClickEnd;
            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        void localAxis(InputTouch touch)
        {
            if (blockInput)
                return;

            if (dpadMode == EasyInputConstants.DPAD_MODE.RegisterAlways || isClicking == false)
            {

                //check to see if we've exceeded the deadzone
                if (Mathf.Sqrt(Mathf.Pow(touch.currentTouchPosition.x, 2) + Mathf.Pow(touch.currentTouchPosition.y, 2)) > deadZone)
                {

                    horizontal = touch.currentTouchPosition.x * sensitivity * Time.deltaTime * 10f;
                    vertical = touch.currentTouchPosition.y * sensitivity * Time.deltaTime * 10f;
                }
                else
                {
                    horizontal = 0f;
                    vertical = 0f;
                }

                actionVector3 = EasyInputUtilities.getControllerVector3(horizontal, vertical, EasyInputConstants.AXIS.XAxis, EasyInputConstants.AXIS.ZAxis);

                previousPos = transform.position;

                transform.Translate(actionVector3,lookObject.transform);

                //we want the x and z from the translate but not the y if we are looking up/down
                previousPos.x = transform.position.x;
                previousPos.z = transform.position.z;

                transform.position = previousPos;


                
            }
        }

        void localClickStart(ButtonClick click)
        {
            if (click.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTouchClick)
            {
                isClicking = true;
            }
        }

        void localClickEnd(ButtonClick click)
        {
            if (click.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTouchClick)
            {
                isClicking = false;
            }
        }

        public override void blockMovement()
        {
            blockInput = true;
        }

        public override void unblockMovement()
        {
            blockInput = false;
        }


    }

}

