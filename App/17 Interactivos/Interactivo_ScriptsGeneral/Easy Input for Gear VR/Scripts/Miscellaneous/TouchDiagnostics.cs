using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using EasyInputVR.Core;

namespace EasyInputVR.Misc
{

    [AddComponentMenu("EasyInputGearVR/Miscellaneous/TouchDiagnostics")]
    public class TouchDiagnostics : MonoBehaviour
    {

        public Text touchStartValue;
        public Text touchEndValue;
        public Text touchValue;
        public Text LongStartValue;
        public Text LongEndValue;
        public Text LongValue;
        public Text quickEndValue;
        public Text doubleEndValue;
        public Text swipeValue;

        public Text HMDaccelXValue;
        public Text HMDaccelYValue;
        public Text HMDaccelZValue;
        public Text HMDTouchXValue;
        public Text HMDTouchYValue;
        public Text GearVRBackValue;
        public Text GearVRClickValue;
        public Text GearVRTriggerValue;
        public Text controllerTouchXValue;
        public Text controllerTouchYValue;

        bool touching;
        bool longTouching;
        bool touchpadClick;
        bool triggerClick;

        void OnEnable()
        {
            EasyInputHelper.On_TouchStart += localTouchStart;
            EasyInputHelper.On_TouchEnd += localTouchEnd;
            EasyInputHelper.On_Touch += localTouch;
            EasyInputHelper.On_LongTouchStart += localLongTouchStart;
            EasyInputHelper.On_LongTouchEnd += localLongTouchEnd;
            EasyInputHelper.On_LongTouch += localLongTouch;
            EasyInputHelper.On_QuickTouchEnd += localQuickTouchEnd;
            EasyInputHelper.On_DoubleTouchEnd += localDoubleTouchEnd;
            EasyInputHelper.On_SwipeDetected += localSwipe;
            EasyInputHelper.On_Click += localClick;
        }

        void OnDestroy()
        {
            EasyInputHelper.On_TouchStart -= localTouchStart;
            EasyInputHelper.On_TouchEnd -= localTouchEnd;
            EasyInputHelper.On_Touch -= localTouch;
            EasyInputHelper.On_LongTouchStart -= localLongTouchStart;
            EasyInputHelper.On_LongTouchEnd -= localLongTouchEnd;
            EasyInputHelper.On_LongTouch -= localLongTouch;
            EasyInputHelper.On_QuickTouchEnd -= localQuickTouchEnd;
            EasyInputHelper.On_DoubleTouchEnd -= localDoubleTouchEnd;
            EasyInputHelper.On_SwipeDetected -= localSwipe;
            EasyInputHelper.On_Click -= localClick;
        }

        void Awake()
        {
            //OVRTouchpad.Create();
        }

        void Update()
        {
            if (!touching)
            {
                touchValue.text = "";
                HMDTouchXValue.text = "";
                HMDTouchYValue.text = "";
                controllerTouchXValue.text = "";
                controllerTouchYValue.text = "";
            }
            else
            {
                touchValue.text = "Fired";
                touching = false;
            }

            if (!longTouching)
                LongValue.text = "";
            else
            {
                LongValue.text = "Fired";
                longTouching = false;
            }

            if (!touchpadClick)
                GearVRClickValue.text = "";
            else
            {
                GearVRClickValue.text = "Fired";
                touchpadClick = false;
            }

            if (!triggerClick)
                GearVRTriggerValue.text = "";
            else
            {
                GearVRTriggerValue.text = "Fired";
                triggerClick = false;
            }

            HMDaccelXValue.text = Input.acceleration.x.ToString();
            HMDaccelYValue.text = Input.acceleration.y.ToString();
            HMDaccelZValue.text = Input.acceleration.z.ToString();
            GearVRBackValue.text = (Input.GetKey(KeyCode.Escape)) ? "Fired" : "";

        }



        void localTouchStart(InputTouch touch)
        {
            touchStartValue.text = "Fired";
            StartCoroutine(ClearTextTouchStart(touchStartValue, .5f));
        }

        IEnumerator ClearTextTouchStart(Text textObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            textObject.text = "";
        }

        void localTouchEnd(InputTouch touch)
        {
            touchEndValue.text = "Fired";
            StartCoroutine(ClearTextTouchEnd(touchEndValue, .5f));
        }

        IEnumerator ClearTextTouchEnd(Text textObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            textObject.text = "";
        }

        void localTouch(InputTouch touch)
        {
            touching = true;
            if (EasyInputHelper.touchDevice == EasyInputConstants.TOUCH_DEVICE.MotionController)
            {
                controllerTouchXValue.text = touch.currentTouchPosition.x.ToString();
                controllerTouchYValue.text = touch.currentTouchPosition.y.ToString();
                HMDTouchXValue.text = "";
                HMDTouchYValue.text = "";
            }
            else if (EasyInputHelper.touchDevice == EasyInputConstants.TOUCH_DEVICE.HMD)
            {
                HMDTouchXValue.text = touch.currentTouchPosition.x.ToString();
                HMDTouchYValue.text = touch.currentTouchPosition.y.ToString();
                controllerTouchXValue.text = "";
                controllerTouchYValue.text = "";

            }
        }

        IEnumerator ClearTextTouch(Text textObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            textObject.text = "";
        }

        void localLongTouchStart(InputTouch touch)
        {
            LongStartValue.text = "Fired";
            StartCoroutine(ClearLongTextTouchStart(LongStartValue, .5f));
        }

        IEnumerator ClearLongTextTouchStart(Text textObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            textObject.text = "";
        }

        void localLongTouchEnd(InputTouch touch)
        {
            LongEndValue.text = "Fired";
            StartCoroutine(ClearLongTextTouchEnd(LongEndValue, .5f));
        }

        IEnumerator ClearLongTextTouchEnd(Text textObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            textObject.text = "";
        }

        void localLongTouch(InputTouch touch)
        {
            longTouching = true;
        }

        void localQuickTouchEnd(InputTouch touch)
        {
            quickEndValue.text = "Fired";
            StartCoroutine(ClearQuickTextTouchEnd(quickEndValue, .5f));
        }

        IEnumerator ClearQuickTextTouchEnd(Text textObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            textObject.text = "";
        }

        void localDoubleTouchEnd(InputTouch touch)
        {
            doubleEndValue.text = "Fired";
            StartCoroutine(ClearDoubleTextTouchEnd(doubleEndValue, .5f));
        }

        IEnumerator ClearDoubleTextTouchEnd(Text textObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            textObject.text = "";
        }

        void localSwipe(InputTouch touch)
        {
            swipeValue.text = "Fired";
            StartCoroutine(ClearSwipeTextTouchEnd(swipeValue, .5f));
        }

        IEnumerator ClearSwipeTextTouchEnd(Text textObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            textObject.text = "";
        }

        void localClick(ButtonClick button)
        {
            if (button.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTouchClick)
            {
                touchpadClick = true;
            }
            else if (button.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTrigger)
            {
                triggerClick = true;

            }

        }

    }
}
