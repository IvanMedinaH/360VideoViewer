using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using EasyInputVR.Core;

namespace EasyInputVR.Misc
{

    [AddComponentMenu("EasyInputGearVR/Miscellaneous/ControllerDiagnostics")]
    public class ControllerDiagnostics : MonoBehaviour
    {
        //buttons
        public Text clickStartValue;
        public Text clickEndValue;
        public Text clickValue;
        public Text LongStartValue;
        public Text LongEndValue;
        public Text LongValue;
        public Text quickEndValue;
        public Text doubleEndValue;

        //axis
        public Text leftStickValue;
        public Text rightStickValue;
        public Text dpadValue;
        public Text leftTriggerValue;
        public Text rightTriggerValue;


        bool touching;
        bool longTouching;
        bool leftStick;
        bool rightStick;
        bool dPad;
        bool leftTrigger;
        bool rightTrigger;

        void OnEnable()
        {
            //buttons
            EasyInputHelper.On_ClickStart += localTouchStart;
            EasyInputHelper.On_ClickEnd += localTouchEnd;
            EasyInputHelper.On_Click += localTouch;
            EasyInputHelper.On_LongClickStart += localLongTouchStart;
            EasyInputHelper.On_LongClickEnd += localLongTouchEnd;
            EasyInputHelper.On_LongClick += localLongTouch;
            EasyInputHelper.On_QuickClickEnd += localQuickTouchEnd;
            EasyInputHelper.On_DoubleClickEnd += localDoubleTouchEnd;

            //axis
            EasyInputHelper.On_LeftStick += localLeftStick;
            EasyInputHelper.On_RightStick += localRightStick;
            EasyInputHelper.On_Dpad += localDpad;
            EasyInputHelper.On_LeftTrigger += localLeftTrigger;
            EasyInputHelper.On_RightTrigger += localRightTrigger;

        }

        void OnDestroy()
        {
            EasyInputHelper.On_ClickStart -= localTouchStart;
            EasyInputHelper.On_ClickEnd -= localTouchEnd;
            EasyInputHelper.On_Click -= localTouch;
            EasyInputHelper.On_LongClickStart -= localLongTouchStart;
            EasyInputHelper.On_LongClickEnd -= localLongTouchEnd;
            EasyInputHelper.On_LongClick -= localLongTouch;
            EasyInputHelper.On_QuickClickEnd -= localQuickTouchEnd;
            EasyInputHelper.On_DoubleClickEnd -= localDoubleTouchEnd;

            //axis
            EasyInputHelper.On_LeftStick -= localLeftStick;
            EasyInputHelper.On_RightStick -= localRightStick;
            EasyInputHelper.On_Dpad -= localDpad;
            EasyInputHelper.On_LeftTrigger -= localLeftTrigger;
            EasyInputHelper.On_RightTrigger -= localRightTrigger;
        }

        void Update()
        {
            if (!touching)
                clickValue.text = "";
            else
            {
                clickValue.text = "Fired";
                touching = false;
            }

            if (!longTouching)
                LongValue.text = "";
            else
            {
                LongValue.text = "Fired";
                longTouching = false;
            }

            if (!leftStick)
                leftStickValue.text = "";
            else
            {
                leftStickValue.text = "Pushed";
                leftStick = false;
            }

            if (!rightStick)
                rightStickValue.text = "";
            else
            {
                rightStickValue.text = "Pushed";
                rightStick = false;
            }

            if (!dPad)
                dpadValue.text = "";
            else
            {
                dpadValue.text = "Pushed";
                dPad = false;
            }

            if (!leftTrigger)
                leftTriggerValue.text = "";
            else
            {
                leftTriggerValue.text = "Pushed";
                leftTrigger = false;
            }

            if (!rightTrigger)
                rightTriggerValue.text = "";
            else
            {
                rightTriggerValue.text = "Pushed";
                rightTrigger = false;
            }

        }



        void localTouchStart(ButtonClick touch)
        {
            clickStartValue.text = "Fired";
            StartCoroutine(ClearTextTouchStart(clickStartValue, .5f));
        }

        IEnumerator ClearTextTouchStart(Text textObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            textObject.text = "";
        }

        void localTouchEnd(ButtonClick touch)
        {
            clickEndValue.text = "Fired";
            StartCoroutine(ClearTextTouchEnd(clickEndValue, .5f));
        }

        IEnumerator ClearTextTouchEnd(Text textObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            textObject.text = "";
        }

        void localTouch(ButtonClick touch)
        {
            touching = true;
        }

        void localLongTouchStart(ButtonClick touch)
        {
            LongStartValue.text = "Fired";
            StartCoroutine(ClearLongTextTouchStart(LongStartValue, .5f));
        }

        IEnumerator ClearLongTextTouchStart(Text textObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            textObject.text = "";
        }

        void localLongTouchEnd(ButtonClick touch)
        {
            LongEndValue.text = "Fired";
            StartCoroutine(ClearLongTextTouchEnd(LongEndValue, .5f));
        }

        IEnumerator ClearLongTextTouchEnd(Text textObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            textObject.text = "";
        }

        void localLongTouch(ButtonClick touch)
        {
            longTouching = true;
        }

        void localQuickTouchEnd(ButtonClick touch)
        {
            quickEndValue.text = "Fired";
            StartCoroutine(ClearQuickTextTouchEnd(quickEndValue, .5f));
        }

        IEnumerator ClearQuickTextTouchEnd(Text textObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            textObject.text = "";
        }

        void localDoubleTouchEnd(ButtonClick touch)
        {
            doubleEndValue.text = "Fired";
            StartCoroutine(ClearDoubleTextTouchEnd(doubleEndValue, .5f));
        }

        IEnumerator ClearDoubleTextTouchEnd(Text textObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            textObject.text = "";
        }

        void localLeftStick(ControllerAxis axis)
        {
            if (axis.axisValue.x > .05f || axis.axisValue.y > .05f || axis.axisValue.x < -.05f || axis.axisValue.y < -.05f)
            {
                leftStick = true;
            }
        }

        void localRightStick(ControllerAxis axis)
        {
            if (axis.axisValue.x > .05f || axis.axisValue.y > .05f || axis.axisValue.x < -.05f || axis.axisValue.y < -.05f)
            {
                rightStick = true;
            }
        }

        void localDpad(ControllerAxis axis)
        {
            if (axis.axisValue.x > .05f || axis.axisValue.y > .05f || axis.axisValue.x < -.05f || axis.axisValue.y < -.05f)
            {
                dPad = true;
            }
        }

        void localLeftTrigger(ControllerAxis axis)
        {
            if (axis.axisValue.x > .05f || axis.axisValue.y > .05f || axis.axisValue.x < -.05f || axis.axisValue.y < -.05f)
            {
                leftTrigger = true;
            }
        }

        void localRightTrigger(ControllerAxis axis)
        {
            if (axis.axisValue.x > .05f || axis.axisValue.y > .05f || axis.axisValue.x < -.05f || axis.axisValue.y < -.05f)
            {
                rightTrigger = true;
            }
        }

    }
}
