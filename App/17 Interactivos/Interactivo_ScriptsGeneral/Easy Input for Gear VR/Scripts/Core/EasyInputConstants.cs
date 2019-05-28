using UnityEngine;
using System.Collections;
using System;
namespace EasyInputVR.Core
{
    public static class EasyInputConstants
    {
        //enums
        public enum CONTROLLER_AXIS { LeftStick = 0, RightStick = 1, DPad = 2, LeftTrigger = 3, RightTrigger = 4 };
        public enum CONTROLLER_BUTTON { AButton = 0, BButton = 1, XButton = 2, YButton = 3, LeftBumper = 4, RightBumper = 5, StartButton = 6, LeftStickPush = 7, RightStickPush = 8, Back = 9, GearVRTouchClick = 10, GearVRTrigger = 11, GearVRHMDPadTap = 12 };
        public enum ACTION_CONDITION { GearVRTouchClickPressed = 10, GearVRTriggerPressed = 11, GearVRHmdPressed = 12, TouchpadTouched = 13};
        public enum BUTTON_CONDITION { GearVRTouchClickPressed = 10, GearVRTriggerPressed = 11, GearVRHmdPressed = 12};
        public enum TELEPORT_MODE {AlwaysShowLaser = 0, ShowLaserOnConditionStart = 1 };
        public enum DIRECTION_MODE {FourDirectionsOnly = 0, EightDirectionsWithDiagonals = 1 };
        public enum COMPONENT_AXIS { XAxis = 0, YAxis = 1, ZAxis = 2 };
        public enum TOUCH_DEVICE { HMD = 0, MotionController = 1};
        public enum TOUCH_TYPE { Miscellaneous = 0, QuickPress = 1, LongPress = 2, DoublePress = 3, Swipe = 4};
        public enum CLICK_TYPE { Miscellaneous = 0, QuickPress = 1, LongPress = 2, DoublePress = 3};
        public enum SWIPE_TYPE { None = 0, Left = 1, Right = 2, Up = 3, Down = 4};
        public enum AXIS { XAxis = 0, YAxis = 1, ZAxis = 2, NegativeXAxis = 3, NegativeYAxis = 4, NegativeZAxis = 5, None = 6 };
        public enum ACTION_TYPE { Position = 0, Rotation = 1, LocalPosition = 2, LocalRotation = 3, LocalScale = 4 };
        public enum INPUT_MODULE_BUTTON_MODE {FireAtRepeatRate = 0, FireOnceAtButtonUp = 1, FireOnceAtButtonDown = 2 };
        public enum DPAD_MODE {RegisterAlways = 0, RegisterOnlyWhenNotClicking = 1 };
        public enum REGISTER_MODE {RegisterAlways = 0, RegisterOnlyWhenClicking = 1 };
        public enum ROTATION_MODE {None = 0, TwistLocalXAxis =1, TwistLocalYAxis =2, TwistLocalZAxis = 3 };
        public enum DROP_MODE {Drop=0, ThrowLatestVelocity = 1, ThrowAvgLastTwoFrames = 2 }


        //constants
        public static Vector2 NOT_TOUCHING = new Vector2(-99f,-99f);
        public static float NO_TIMESTAMP = -99f;
        public static Vector3 NOT_VALID = new Vector3(-999f, -999f, -999f);
        public static float MINUMUM_NOISE_THRESHOLD = .02f;
        public static int CONSECUTIVE_FRAMES_MOVE_VELOCITY_TO_ZERO = 3;

        //Not absolutely necessary but I don't want to generate UnNecessary Garbage as immutable strings change
        public const string P1_LEFTSTICK_HORIZONTAL = "EIVR_LeftStick_Horizontal";
        public const string P1_LEFTSTICK_VERTICAL = "EIVR_LeftStick_Vertical";
        public const string P1_RIGHTSTICK_HORIZONTAL = "EIVR_RightStick_Horizontal";
        public const string P1_RIGHTSTICK_VERTICAL = "EIVR_RightStick_Vertical";
        public const string P1_DPAD_HORIZONTAL = "EIVR_Dpad_Horizontal";
        public const string P1_DPAD_VERTICAL = "EIVR_Dpad_Vertical";
        public const string P1_LEFTTRIGGER = "EIVR_LeftTrigger";
        public const string P1_RIGHTTRIGGER = "EIVR_RightTrigger";


    }
}
