using UnityEngine;
using System.Collections;
using System;

namespace EasyInputVR.Core
{
    public class InputTouch
    {
        public Vector2 currentTouchPosition = EasyInputConstants.NOT_TOUCHING;
        public Vector2 originalTouchPosition = EasyInputConstants.NOT_TOUCHING;
        public Vector2 previousTouchOriginalPosition = EasyInputConstants.NOT_TOUCHING;
        public float currentTouchBeginTimestamp = EasyInputConstants.NO_TIMESTAMP;
        public float previousTouchBeginTimestamp = EasyInputConstants.NO_TIMESTAMP;
        public EasyInputConstants.TOUCH_TYPE touchType;
        public EasyInputConstants.SWIPE_TYPE swipeType;
    }
}
