using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using EasyInputVR.Core;
using System;
using System.Collections.Generic;

namespace EasyInputVR.StandardControllers
{

    [AddComponentMenu("EasyInputGearVR/Custom Controllers/CustomAxisController")]
    public class CustomAxisController : MonoBehaviour
    {

        //events
        [System.Serializable] public class AxisHandler : UnityEvent<ControllerAxis> { }
        [SerializeField]
        public AxisHandler onLeftStick;
        [SerializeField]
        public AxisHandler onRightStick;
        [SerializeField]
        public AxisHandler onDpad;
        [SerializeField]
        public AxisHandler onLeftTrigger;
        [SerializeField]
        public AxisHandler onRightTrigger;



        void OnEnable()
        {
            EasyInputHelper.On_LeftStick += localLeftStick;
            EasyInputHelper.On_RightStick += localRightStick;
            EasyInputHelper.On_Dpad += localDpad;
            EasyInputHelper.On_LeftTrigger += localLeftTrigger;
            EasyInputHelper.On_RightTrigger += localRightTrigger;

        }

        void OnDestroy()
        {
            EasyInputHelper.On_LeftStick -= localLeftStick;
            EasyInputHelper.On_RightStick -= localRightStick;
            EasyInputHelper.On_Dpad -= localDpad;
            EasyInputHelper.On_LeftTrigger -= localLeftTrigger;
            EasyInputHelper.On_RightTrigger -= localRightTrigger;
        }

        // Update is called once per frame
        void Update()
        {
        }

        void localLeftStick(ControllerAxis axis)
        {
                onLeftStick.Invoke(axis);
        }

        void localRightStick(ControllerAxis axis)
        {
                onRightStick.Invoke(axis);
        }

        void localDpad(ControllerAxis axis)
        {
                onDpad.Invoke(axis);
        }

        void localLeftTrigger(ControllerAxis axis)
        {
                onLeftTrigger.Invoke(axis);
        }

        void localRightTrigger(ControllerAxis axis)
        {
                onRightTrigger.Invoke(axis);
        }




    }

}

