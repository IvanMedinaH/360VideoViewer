using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using EasyInputVR.Core;
using System;
using System.Collections.Generic;

namespace EasyInputVR.StandardControllers
{

    [AddComponentMenu("EasyInputGearVR/Custom Controllers/CustomTouchInputController")]
    public class CustomTouchInputController : MonoBehaviour
    {
        //events
        [System.Serializable] public class TouchHandler : UnityEvent<InputTouch> { }
        [SerializeField]
        public TouchHandler onTouchStart;
        [SerializeField]
        public TouchHandler onTouchEnd;
        [SerializeField]
        public TouchHandler onTouch;
        [SerializeField]
        public TouchHandler onQuickTouchEnd;
        [SerializeField]
        public TouchHandler onDoubleTouchEnd;
        [SerializeField]
        public TouchHandler onLongTouchStart;
        [SerializeField]
        public TouchHandler onLongTouchEnd;
        [SerializeField]
        public TouchHandler onLongTouch;
        [SerializeField]
        public TouchHandler onSwipe;



        void OnEnable()
        {
            EasyInputHelper.On_Touch += localButtonClick;
            EasyInputHelper.On_TouchEnd += localButtonClickEnd;
            EasyInputHelper.On_TouchStart += localButtonClickStart;
            EasyInputHelper.On_DoubleTouchEnd += localButtonDoubleClickEnd;
            EasyInputHelper.On_LongTouch += localButtonLongClick;
            EasyInputHelper.On_LongTouchEnd += localButtonLongClickEnd;
            EasyInputHelper.On_LongTouchStart += localButtonLongClickStart;
            EasyInputHelper.On_QuickTouchEnd += localButtonQuickClickEnd;
            EasyInputHelper.On_SwipeDetected += localSwipe;
        }

        void OnDestroy()
        {
            EasyInputHelper.On_Touch -= localButtonClick;
            EasyInputHelper.On_TouchEnd -= localButtonClickEnd;
            EasyInputHelper.On_TouchStart -= localButtonClickStart;
            EasyInputHelper.On_DoubleTouchEnd -= localButtonDoubleClickEnd;
            EasyInputHelper.On_LongTouch -= localButtonLongClick;
            EasyInputHelper.On_LongTouchEnd -= localButtonLongClickEnd;
            EasyInputHelper.On_LongTouchStart -= localButtonLongClickStart;
            EasyInputHelper.On_QuickTouchEnd -= localButtonQuickClickEnd;
            EasyInputHelper.On_SwipeDetected -= localSwipe;
        }

        // Update is called once per frame
        void Update()
        {
        }

        void localButtonClick(InputTouch button)
        {
            onTouch.Invoke(button);
        }

        void localButtonClickStart(InputTouch button)
        {
            onTouchStart.Invoke(button);
        }

        void localButtonClickEnd(InputTouch button)
        {
            onTouchEnd.Invoke(button);
        }

        void localButtonQuickClickEnd(InputTouch button)
        {
            onQuickTouchEnd.Invoke(button);
        }

        void localButtonDoubleClickEnd(InputTouch button)
        {
            onDoubleTouchEnd.Invoke(button);
        }

        void localButtonLongClick(InputTouch button)
        {
            onLongTouch.Invoke(button);
        }

        void localButtonLongClickStart(InputTouch button)
        {
            onLongTouchStart.Invoke(button);
        }

        void localButtonLongClickEnd(InputTouch button)
        {
            onLongTouchEnd.Invoke(button);
        }

        void localSwipe(InputTouch button)
        {
            onSwipe.Invoke(button);
        }
    }

}

