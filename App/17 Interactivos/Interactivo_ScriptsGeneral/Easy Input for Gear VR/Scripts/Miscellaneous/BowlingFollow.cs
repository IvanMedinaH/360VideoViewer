using UnityEngine;
using System.Collections;
using EasyInputVR.Core;
using System;

namespace EasyInputVR.StandardControllers
{

    [AddComponentMenu("EasyInputGearVR/Miscellaneous/BowlingFollow")]
    public class BowlingFollow : MonoBehaviour
    {
        public EasyInputConstants.AXIS axisHorizontal = EasyInputConstants.AXIS.XAxis;
        public EasyInputConstants.AXIS axisVertical = EasyInputConstants.AXIS.YAxis;
        public EasyInputConstants.ACTION_TYPE action = EasyInputConstants.ACTION_TYPE.Position;
        public float sensitivity = 1f;
        public Camera followingCamera;
        public Transform aimArrow;
        public float laneRange = 0f;

        //runtime variables
        Vector2 lastFrameTouch = EasyInputConstants.NOT_TOUCHING;
        Vector3 actionVector3;
        float horizontal;
        float vertical;
        Vector3 temp;
        bool preventControl = false;


        void OnEnable()
        {
            EasyInputHelper.On_Touch += localAxis;
            EasyInputHelper.On_TouchEnd += localAxisEnd;
        }

        void OnDestroy()
        {
            EasyInputHelper.On_Touch -= localAxis;
            EasyInputHelper.On_TouchEnd -= localAxisEnd;
        }

        // Update is called once per frame
        void Update()
        {
        }

        void localAxisEnd(InputTouch touch)
        {
            //we just ended a touch so reset the last frame
            lastFrameTouch = EasyInputConstants.NOT_TOUCHING;
        }

        void localAxis(InputTouch touch)
        {
            //check to see if we want to effect anything
            if (preventControl)
                return;

            //first check to see if this is the first frame
            if (lastFrameTouch == EasyInputConstants.NOT_TOUCHING)
            {
                lastFrameTouch = touch.currentTouchPosition;
                return;
            }

            //otherwise is a continuation
            horizontal = (touch.currentTouchPosition.x - lastFrameTouch.x) * sensitivity * Time.deltaTime * 100f;
            vertical = (touch.currentTouchPosition.y - lastFrameTouch.y) * sensitivity * Time.deltaTime * 100f;
            actionVector3 = EasyInputUtilities.getControllerVector3(horizontal, vertical, axisHorizontal, axisVertical);

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

            //bowling ball lane range
            temp = transform.position;
            if (transform.position.x > laneRange)
                temp.x = laneRange;
            else if (transform.position.x < -laneRange)
                temp.x = -laneRange;

            transform.position = temp;


            //move the camera to follow
            temp.x = transform.position.x;
            temp.y = 3.06f;
            temp.z = -6f;
            followingCamera.transform.position = temp;

            //also move the aim arrow
            temp = aimArrow.position;
            temp.x = transform.position.x;
            aimArrow.position = temp;

            lastFrameTouch = touch.currentTouchPosition;
        }

        public void stopFollow()
        {
            aimArrow.gameObject.SetActive(false);
            preventControl = true;
        }


    }

}

