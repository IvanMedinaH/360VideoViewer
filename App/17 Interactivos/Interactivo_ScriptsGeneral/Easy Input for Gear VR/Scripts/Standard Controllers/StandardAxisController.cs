using UnityEngine;
using System.Collections;
using EasyInputVR.Core;
using System;

namespace EasyInputVR.StandardControllers
{

    [AddComponentMenu("EasyInputGearVR/Standard Controllers/StandardAxisController")]
    public class StandardAxisController : MonoBehaviour
    {
        public EasyInputConstants.CONTROLLER_AXIS control = EasyInputConstants.CONTROLLER_AXIS.LeftStick;
        public EasyInputConstants.AXIS axisHorizontal = EasyInputConstants.AXIS.XAxis;
        public EasyInputConstants.AXIS axisVertical = EasyInputConstants.AXIS.YAxis;
        public EasyInputConstants.ACTION_TYPE action = EasyInputConstants.ACTION_TYPE.Position;
        public float sensitivity = .01f;

        //runtime variables
        Vector3 actionVector3;
        float horizontal;
        float vertical;


        void OnEnable()
        {
            switch (control)
            {
                case EasyInputConstants.CONTROLLER_AXIS.LeftStick:
                    EasyInputHelper.On_LeftStick += localAxis;
                    break;
                case EasyInputConstants.CONTROLLER_AXIS.RightStick:
                    EasyInputHelper.On_RightStick += localAxis;
                    break;
                case EasyInputConstants.CONTROLLER_AXIS.DPad:
                    EasyInputHelper.On_Dpad += localAxis;
                    break;
                case EasyInputConstants.CONTROLLER_AXIS.LeftTrigger:
                    EasyInputHelper.On_LeftTrigger += localAxis;
                    break;
                case EasyInputConstants.CONTROLLER_AXIS.RightTrigger:
                    EasyInputHelper.On_RightTrigger += localAxis;
                    break;
                default:
                    break;

            }
        }

        void OnDestroy()
        {
            switch (control)
            {
                case EasyInputConstants.CONTROLLER_AXIS.LeftStick:
                    EasyInputHelper.On_LeftStick -= localAxis;
                    break;
                case EasyInputConstants.CONTROLLER_AXIS.RightStick:
                    EasyInputHelper.On_RightStick -= localAxis;
                    break;
                case EasyInputConstants.CONTROLLER_AXIS.DPad:
                    EasyInputHelper.On_Dpad -= localAxis;
                    break;
                case EasyInputConstants.CONTROLLER_AXIS.LeftTrigger:
                    EasyInputHelper.On_LeftTrigger -= localAxis;
                    break;
                case EasyInputConstants.CONTROLLER_AXIS.RightTrigger:
                    EasyInputHelper.On_RightTrigger -= localAxis;
                    break;
                default:
                    break;

            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        void localAxis (ControllerAxis axis)
        {
                horizontal = axis.axisValue.x * sensitivity * Time.deltaTime * 100f;
                vertical = axis.axisValue.y * sensitivity * Time.deltaTime * 100f;
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
           


        }
    }

}

