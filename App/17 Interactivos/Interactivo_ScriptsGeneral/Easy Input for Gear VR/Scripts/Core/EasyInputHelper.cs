using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

namespace EasyInputVR.Core
{
    [AddComponentMenu("EasyInputGearVR/Manager/EasyInputHelper")]
    public class EasyInputHelper : MonoBehaviour
    {

        //inspector

        [Tooltip("The maximum distance of movement to be considered for the quickpress, longpress, and doublepress")]
        public float maxPressLength = .1f;
        [Tooltip("The distance of movement to fire a swipe event. Only one event will be fired when you reach this distance")]
        public float requiredSwipeLength = .6f;
        [Tooltip("The longest you can hold in seconds and still be considered a quick press")]
        public float maxQuickTapTime = .2f;
        [Tooltip("The shortest amount of time in seconds that will be considered a long press")]
        public float minLongTapTime = 1f;
        [Tooltip("The longest double tap in seconds and still be considered a double press")]
        public float maxDoubleTapTime = .5f;
        //instance
        public static EasyInputHelper mInstance;

        //delegates

        //touch

        //fired once on beginning of any kind of touch
        public delegate void onTouchStartHandler(InputTouch touch);

        //always fired when being touched
        public delegate void onTouchHandler(InputTouch touch);

        //fired once at end of any kind of touch
        public delegate void onTouchEndHandler(InputTouch touch);

        //fired once at the end of a quick touch only
        public delegate void quickTouchEndHandler(InputTouch touch);

        //fired once at the beginning of a long touch
        public delegate void longTouchStartHandler(InputTouch touch);

        //always fired during long touch
        public delegate void longTouchHandler(InputTouch touch);

        //fired once at end of long touch
        public delegate void longTouchEndHandler(InputTouch touch);

        //fired once at end of double touch
        public delegate void doubleTouchEndHandler(InputTouch touch);

        //fired once when a swipe is detected by traveling distance (fired when distance is reached)
        public delegate void swipeDistanceHandler(InputTouch touch);

        //buttons (controller or remote)
        public delegate void onClickStartHandler(ButtonClick button); //change this to a button class later
        public delegate void onClickHandler(ButtonClick button);
        public delegate void onClickEndHandler(ButtonClick button);
        public delegate void quickClickEndHandler(ButtonClick button);
        public delegate void longClickStartHandler(ButtonClick button);
        public delegate void longClickHandler(ButtonClick button);
        public delegate void longClickEndHandler(ButtonClick button);
        public delegate void doubleClickEndHandler(ButtonClick button);

        //axis (controller or remote)
        public delegate void onAxisHandler(ControllerAxis axis);

        //motion
        public delegate void MotionHandler(Motion motion);

        //events
        //touch
        public static event onTouchStartHandler On_TouchStart;
        public static event onTouchHandler On_Touch;
        public static event onTouchEndHandler On_TouchEnd;
        public static event quickTouchEndHandler On_QuickTouchEnd;
        public static event longTouchStartHandler On_LongTouchStart;
        public static event longTouchHandler On_LongTouch;
        public static event longTouchEndHandler On_LongTouchEnd;
        public static event doubleTouchEndHandler On_DoubleTouchEnd;
        public static event swipeDistanceHandler On_SwipeDetected;

        //buttons
        public static event onClickStartHandler On_ClickStart;
        public static event onClickHandler On_Click;
        public static event onClickEndHandler On_ClickEnd;
        public static event quickClickEndHandler On_QuickClickEnd;
        public static event longClickStartHandler On_LongClickStart;
        public static event longClickHandler On_LongClick;
        public static event longClickEndHandler On_LongClickEnd;
        public static event doubleClickEndHandler On_DoubleClickEnd;

        //axis
        public static event onAxisHandler On_LeftStick;
        public static event onAxisHandler On_RightStick;
        public static event onAxisHandler On_Dpad;
        public static event onAxisHandler On_LeftTrigger;
        public static event onAxisHandler On_RightTrigger;

        //motion
        public static event MotionHandler On_Motion;

        public static bool isGearVR = false;
        public static EasyInputConstants.TOUCH_DEVICE touchDevice;

        //static reset
        public static void resetMotion()
        {
            mInstance.myMotion.lastResetTimestamp = Time.time;
            mInstance.myMotion.posSinceLastReset = Vector3.zero;
            mInstance.myMotion.lastFrameVel = Vector3.zero;
            mInstance.myMotion.totalAngularVelSinceLastReset = Vector3.zero;
            mInstance.myMotion.totalVelSinceLastReset = Vector3.zero;
        }

        public static void recenterController()
        {
#if UNITY_EDITOR
            mInstance.myMotion.currentOrientationEuler = Vector3.zero;
            mInstance.myMotion.currentOrientation = Quaternion.Euler(mInstance.myMotion.currentOrientationEuler);
#endif
#if !UNITY_EDITOR && UNITY_ANDROID
            OVRInput.RecenterController();
#endif
        }

        public static Camera returnUICamera()
        {
            EasyInputModule[] inputmodules = Resources.FindObjectsOfTypeAll<EasyInputModule>();
            foreach (EasyInputModule inputmodule in inputmodules)
            {
                return inputmodule.UICamera;
            }
            return Camera.main;
        }


        //runtime variables
        Touch touch;
        bool swipeAllowed;
        bool currentlyTouching;
        bool previousTouching;
        Vector3 temp;
        Vector2 currentMousePos;
        Vector2 lastFrameMousePos;
        Vector2 delta;
        Vector3 OrientationDelta = Vector3.zero;
        bool isController;
        bool isCamera;
        int framesVelocitySameX = 0, framesVelocitySameY = 0, framesVelocitySameZ = 0;


        //supporting variables
        InputTouch myTouch;
        Motion myMotion;


        //supporting controller
        ControllerAxis[] axisArray;
        ButtonClick[] buttonArray;



        void Awake()
        {
            mInstance = this;
            axisArray = new ControllerAxis[5]; //5 axis per controller
            buttonArray = new ButtonClick[13]; //13 buttons per controller
            myTouch = new InputTouch();
            myMotion = new Motion();
            myMotion.currentOrientation = Quaternion.identity;

            //even though destroy gets called previous string delegates may still be active. reset them
            On_TouchStart = null;
            On_Touch = null;
            On_TouchEnd = null;
            On_QuickTouchEnd = null;
            On_LongTouchStart = null;
            On_LongTouch = null;
            On_LongTouchEnd = null;
            On_DoubleTouchEnd = null;
            On_SwipeDetected = null;

            //buttons
            On_ClickStart = null;
            On_Click = null;
            On_ClickEnd = null;
            On_QuickClickEnd = null;
            On_LongClickStart = null;
            On_LongClick = null;
            On_LongClickEnd = null;
            On_DoubleClickEnd = null;

            //axis
            On_LeftStick = null;
            On_RightStick = null;
            On_Dpad = null;
            On_LeftTrigger = null;
            On_RightTrigger = null;

            //motion
            On_Motion = null;

#if !UNITY_EDITOR && UNITY_ANDROID
#if UNITY_5_3
            var device = UnityEngine.VR.VRSettings.loadedDevice;
            if (device == UnityEngine.VR.VRDeviceType.Oculus)
            {
                isGearVR = true;
                //add OVRManager
                this.gameObject.AddComponent<OVRManager>();
            }
#endif
#if !UNITY_5_3 && (UNITY_5_4 || UNITY_5_5 || UNITY_5_6 || UNITY_5_7 || UNITY_2017_1)
            var devices = UnityEngine.VR.VRSettings.supportedDevices;
            foreach (var device in devices)
            {
                if (device == "Oculus")
                {
                    isGearVR = true;
                    //add OVRManager
                    this.gameObject.AddComponent<OVRManager>();
                }
            }
#endif
#if !UNITY_5_3 && !UNITY_5_4 && !UNITY_5_5 && !UNITY_5_6 && !UNITY_5_7 && !UNITY_2017_1
            var devices = UnityEngine.XR.XRSettings.supportedDevices;
            foreach (var device in devices)
            {
                if (device == "Oculus")
                {
                    isGearVR = true;
                    //add OVRManager
                    this.gameObject.AddComponent<OVRManager>();
                }
            }
#endif
#endif
        }

        // Use this for initialization
        void Start()
        {
            //setup the controller buttons
            setupControllerButtons();

            setupControllerAxes();

        }

        // Update is called once per frame
        void Update()
        {
            populateMyTouch();
            processTouchEvents();
            processControllerButtonEvents();
            processControllerAxisEvents();
            processMotionEvents();
        }

        void populateMyTouch()
        {
#if UNITY_EDITOR
            currentlyTouching = (Input.GetMouseButton(0) && !Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightControl));
            touchDevice = (Input.GetKey(KeyCode.H)) ? EasyInputConstants.TOUCH_DEVICE.HMD : EasyInputConstants.TOUCH_DEVICE.MotionController;
            if (currentlyTouching)
            {

                if (!previousTouching)
                {
                    //start of a touch (wasn't touching previous) puts in -1 to 1 range
                    myTouch.originalTouchPosition.x = (((Input.mousePosition.x / (float)Screen.width) - .5f) * 2f);
                    myTouch.originalTouchPosition.y = (((Input.mousePosition.y / (float)Screen.height) - .5f) * 2f);
                    myTouch.currentTouchPosition.x = (((Input.mousePosition.x / (float)Screen.width) - .5f) * 2f);
                    myTouch.currentTouchPosition.y = (((Input.mousePosition.y / (float)Screen.height) - .5f) * 2f);
                    myTouch.currentTouchBeginTimestamp = Time.time;

                }
                else
                {
                    //continue of touch puts in -1 to 1 range
                    myTouch.currentTouchPosition.x = (((Input.mousePosition.x / (float)Screen.width) - .5f) * 2f);
                    myTouch.currentTouchPosition.y = (((Input.mousePosition.y / (float)Screen.height) - .5f) * 2f);
                }
            }
#endif

#if !UNITY_EDITOR && UNITY_ANDROID

                if (isGearVR)
                {
                    currentlyTouching = OVRInput.Get(OVRInput.Touch.PrimaryTouchpad);
                    touchDevice = (OVRInput.GetActiveController() == OVRInput.Controller.LTrackedRemote || OVRInput.GetActiveController() == OVRInput.Controller.RTrackedRemote) ? EasyInputConstants.TOUCH_DEVICE.MotionController : EasyInputConstants.TOUCH_DEVICE.HMD;
                }


            if (currentlyTouching)
                {

                    if (!previousTouching)
                    {
                    //start of a touch (wasn't touching previous)
                        if (isGearVR)
                        {
                            myTouch.originalTouchPosition.x = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).x;
                            myTouch.originalTouchPosition.y = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).y;
                            myTouch.currentTouchPosition.x = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).x;
                            myTouch.currentTouchPosition.y = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).y;
                            myTouch.currentTouchBeginTimestamp = Time.time;
                        }
                    }
                    else
                    {
                        //normal continue of touch
                        if (isGearVR)
                        {
                            myTouch.currentTouchPosition.x = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).x;
                            myTouch.currentTouchPosition.y = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).y;
                        }



                    }
                        


                }
                

#endif
        }

        void processTouchEvents()
        {
            if (currentlyTouching)
            {

                if (!previousTouching)
                {
                    //swipetype always starts at none
                    myTouch.swipeType = EasyInputConstants.SWIPE_TYPE.None;

                    //touchtype starts at quick if new or double if were in the time factor for double press
                    if (myTouch.previousTouchBeginTimestamp != EasyInputConstants.NO_TIMESTAMP
                        && (myTouch.currentTouchBeginTimestamp - myTouch.previousTouchBeginTimestamp) < maxDoubleTapTime
                        && EasyInputUtilities.determineDistance(myTouch.previousTouchOriginalPosition, myTouch.originalTouchPosition) < maxPressLength)
                    {
                        //possible start of a double tap
                        myTouch.touchType = EasyInputConstants.TOUCH_TYPE.DoublePress;
                    }
                    else
                    {
                        myTouch.touchType = EasyInputConstants.TOUCH_TYPE.QuickPress;
                    }

                    //fire off the touch start event and touch event
                    if (On_TouchStart != null)
                        On_TouchStart(myTouch);
                    if (On_Touch != null)
                    {
                        On_Touch(myTouch);
                    }

                }
                else
                {

                    //change to quickpress
                    if (myTouch.touchType == EasyInputConstants.TOUCH_TYPE.Miscellaneous
                        && EasyInputUtilities.determineDistance(myTouch.originalTouchPosition, myTouch.currentTouchPosition) < maxPressLength
                        && Time.time - myTouch.currentTouchBeginTimestamp < maxQuickTapTime)
                    {
                        myTouch.touchType = EasyInputConstants.TOUCH_TYPE.QuickPress;
                    }
                    //change to longpress
                    if ((myTouch.touchType == EasyInputConstants.TOUCH_TYPE.Miscellaneous || myTouch.touchType == EasyInputConstants.TOUCH_TYPE.QuickPress || myTouch.touchType == EasyInputConstants.TOUCH_TYPE.DoublePress) && EasyInputUtilities.determineDistance(myTouch.originalTouchPosition, myTouch.currentTouchPosition) < maxPressLength && Time.time - myTouch.currentTouchBeginTimestamp > minLongTapTime)
                    {
                        myTouch.touchType = EasyInputConstants.TOUCH_TYPE.LongPress;
                        if (On_LongTouchStart != null)
                            On_LongTouchStart(myTouch);
                    }

                    //change to swipe
                    if (EasyInputUtilities.determineDistance(myTouch.originalTouchPosition, myTouch.currentTouchPosition) > requiredSwipeLength)
                    {
                        //if we're changing to a swipe from long press fire the appropriate end event
                        if (myTouch.touchType == EasyInputConstants.TOUCH_TYPE.LongPress)
                        {
                            if (On_LongTouchEnd != null)
                                On_LongTouchEnd(myTouch);
                        }

                        myTouch.touchType = EasyInputConstants.TOUCH_TYPE.Swipe;
                        myTouch.swipeType = EasyInputUtilities.determineSwipeType(myTouch);
                        if (swipeAllowed)
                        {
                            if (On_SwipeDetected != null)
                                On_SwipeDetected(myTouch);
                            swipeAllowed = false;

                        }
                    }

                    //change to miscellaneous
                    if (!(myTouch.touchType == EasyInputConstants.TOUCH_TYPE.Swipe || myTouch.touchType == EasyInputConstants.TOUCH_TYPE.Miscellaneous))
                    {
                        //downgraded from quickpress
                        if (myTouch.touchType == EasyInputConstants.TOUCH_TYPE.QuickPress)
                        {
                            if (EasyInputUtilities.determineDistance(myTouch.originalTouchPosition, myTouch.currentTouchPosition) > maxPressLength)
                            {
                                //we were a quick press but now we've moved to far revert to misc
                                myTouch.touchType = EasyInputConstants.TOUCH_TYPE.Miscellaneous;
                            }
                            if (Time.time - myTouch.currentTouchBeginTimestamp > maxQuickTapTime)
                            {
                                //we were a quick press but now we've taken too long revert to misc
                                myTouch.touchType = EasyInputConstants.TOUCH_TYPE.Miscellaneous;
                            }
                        }

                        //downgraded from longpress
                        else if (myTouch.touchType == EasyInputConstants.TOUCH_TYPE.LongPress)
                        {
                            if (EasyInputUtilities.determineDistance(myTouch.originalTouchPosition, myTouch.currentTouchPosition) > maxPressLength)
                            {
                                //we were a quick press but now we've moved to far revert to misc
                                myTouch.touchType = EasyInputConstants.TOUCH_TYPE.Miscellaneous;
                                if (On_LongTouchEnd != null)
                                    On_LongTouchEnd(myTouch);
                            }
                        }

                        //downgraded from double press
                        else if (myTouch.touchType == EasyInputConstants.TOUCH_TYPE.DoublePress)
                        {
                            if (EasyInputUtilities.determineDistance(myTouch.originalTouchPosition, myTouch.currentTouchPosition) > maxPressLength)
                            {
                                //we were a quick press but now we've moved to far revert to misc
                                myTouch.touchType = EasyInputConstants.TOUCH_TYPE.Miscellaneous;
                            }
                            if (Time.time - myTouch.currentTouchBeginTimestamp > maxDoubleTapTime)
                            {
                                //we were a quick press but now we've taken too long revert to misc
                                myTouch.touchType = EasyInputConstants.TOUCH_TYPE.Miscellaneous;
                            }
                        }
                    }



                    //if touching we always fire off the touch event
                    if (On_Touch != null)
                    {
                        On_Touch(myTouch);
                    }

                    //if touching and currently a long press always fire that too
                    if (myTouch.touchType == EasyInputConstants.TOUCH_TYPE.LongPress)
                    {
                        if (On_LongTouch != null)
                            On_LongTouch(myTouch);
                    }

                }
                previousTouching = true;

            }
            else
            {
                //no current touch 

                //first look at previous values to determine if we need to do anything
                if (previousTouching == true)
                {
                    //we just ended a touch
                    if (On_TouchEnd != null)
                    {
                        On_TouchEnd(myTouch);
                    }
                    

                    //also fire end of quick or long if it was this (remember we don't fire a swipe event on end we fire it
                    //on each time we hit the swipe length
                    if (myTouch.touchType == EasyInputConstants.TOUCH_TYPE.QuickPress)
                    {
                        //double check that we've just ended a quick touch
                        if (Time.time - myTouch.currentTouchBeginTimestamp < maxQuickTapTime)
                            if (On_QuickTouchEnd != null)
                                On_QuickTouchEnd(myTouch);
                    }
                    else if (myTouch.touchType == EasyInputConstants.TOUCH_TYPE.LongPress)
                    {
                        //double check that we've just ended a long touch
                        if (Time.time - myTouch.currentTouchBeginTimestamp > minLongTapTime)
                            if (On_LongTouchEnd != null)
                                On_LongTouchEnd(myTouch);
                    }
                    else if (myTouch.touchType == EasyInputConstants.TOUCH_TYPE.DoublePress)
                    {
                        //double check that we've just ended a double touch
                        if (Time.time - myTouch.previousTouchBeginTimestamp < maxDoubleTapTime)
                            if (On_DoubleTouchEnd != null)
                                On_DoubleTouchEnd(myTouch);
                    }

                    //since we were previously touching set the timestamp
                    myTouch.previousTouchBeginTimestamp = myTouch.currentTouchBeginTimestamp;
                    myTouch.previousTouchOriginalPosition = myTouch.originalTouchPosition;
                }


                //reset the variables since there is no current touch
                myTouch.originalTouchPosition = EasyInputConstants.NOT_TOUCHING;
                myTouch.currentTouchPosition = EasyInputConstants.NOT_TOUCHING;
                myTouch.currentTouchBeginTimestamp = EasyInputConstants.NO_TIMESTAMP;
                myTouch.touchType = EasyInputConstants.TOUCH_TYPE.Miscellaneous;
                myTouch.swipeType = EasyInputConstants.SWIPE_TYPE.None;
                swipeAllowed = true;
                previousTouching = false;
            }
        }

        void setupControllerButtons()
        {
                //a button
                buttonArray[0] = new ButtonClick();
                buttonArray[0].button = EasyInputConstants.CONTROLLER_BUTTON.AButton;

                //b button
                buttonArray[1] = new ButtonClick();
                buttonArray[1].button = EasyInputConstants.CONTROLLER_BUTTON.BButton;

                //x button
                buttonArray[2] = new ButtonClick();
                buttonArray[2].button = EasyInputConstants.CONTROLLER_BUTTON.XButton;

                //y button
                buttonArray[3] = new ButtonClick();
                buttonArray[3].button = EasyInputConstants.CONTROLLER_BUTTON.YButton;

                //lb button
                buttonArray[4] = new ButtonClick();
                buttonArray[4].button = EasyInputConstants.CONTROLLER_BUTTON.LeftBumper;

                //rb button
                buttonArray[5] = new ButtonClick();
                buttonArray[5].button = EasyInputConstants.CONTROLLER_BUTTON.RightBumper;

                //start button
                buttonArray[6] = new ButtonClick();
                buttonArray[6].button = EasyInputConstants.CONTROLLER_BUTTON.StartButton;

                //back button
                buttonArray[7] = new ButtonClick();
                buttonArray[7].button = EasyInputConstants.CONTROLLER_BUTTON.Back;

                //ls click button
                buttonArray[8] = new ButtonClick();
                buttonArray[8].button = EasyInputConstants.CONTROLLER_BUTTON.LeftStickPush;

                //rs click button
                buttonArray[9] = new ButtonClick();
                buttonArray[9].button = EasyInputConstants.CONTROLLER_BUTTON.RightStickPush;

                //ls click button
                buttonArray[10] = new ButtonClick();
                buttonArray[10].button = EasyInputConstants.CONTROLLER_BUTTON.GearVRTouchClick;

                //rs click button
                buttonArray[11] = new ButtonClick();
                buttonArray[11].button = EasyInputConstants.CONTROLLER_BUTTON.GearVRTrigger;

                buttonArray[12] = new ButtonClick();
                buttonArray[12].button = EasyInputConstants.CONTROLLER_BUTTON.GearVRHMDPadTap;



        }

        void setupControllerAxes()
        {
                //left stick
                axisArray[0] = new ControllerAxis();
                axisArray[0].axis = EasyInputConstants.CONTROLLER_AXIS.LeftStick;

                //right stick
                axisArray[1] = new ControllerAxis();
                axisArray[1].axis = EasyInputConstants.CONTROLLER_AXIS.RightStick;

                //dpad
                axisArray[2] = new ControllerAxis();
                axisArray[2].axis = EasyInputConstants.CONTROLLER_AXIS.DPad;

                //left trigger
                axisArray[3] = new ControllerAxis();
                axisArray[3].axis = EasyInputConstants.CONTROLLER_AXIS.LeftTrigger;

                //right trigger
                axisArray[4] = new ControllerAxis();
                axisArray[4].axis = EasyInputConstants.CONTROLLER_AXIS.RightTrigger;

        }

        void processControllerButtonEvents()
        {
            //10 possible buttons
            for (int i = 0; i < 13; i++)
            {
                processSpecificButton(ref buttonArray[i]);
            }

        }

        void processControllerAxisEvents()
        {
            for (int i = 0; i < 5; i++)
            {
                processSpecificAxis(ref axisArray[i]);
            }

        }

        void processMotionEvents()
        {
            if (On_Motion != null || Application.isEditor == true)
                    deriveMotionEvents();
        }

        //only called if there is a subscriber calculates values and fires off the delegate
        void deriveMotionEvents()
        {
            myMotion.lastFrameVel = myMotion.currentVel;

#if UNITY_EDITOR
            
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(0))
            {
                //camera simulation
                isController = false;
                isCamera = true;
                currentMousePos.x = Input.mousePosition.x;
                currentMousePos.y = -Input.mousePosition.y;
                if (lastFrameMousePos != EasyInputConstants.NOT_TOUCHING)
                {
                    delta = currentMousePos - lastFrameMousePos;
                }
                else
                {
                    delta = EasyInputConstants.NOT_TOUCHING;
                }
            }
            //motion simulation
            else if ((Input.GetKey(KeyCode.LeftControl)|| Input.GetKey(KeyCode.RightControl)) && Input.GetMouseButton(0))
            {
                //motion simulation
                isController = true;
                isCamera = false;
                currentMousePos.x = Input.mousePosition.x;
                currentMousePos.y = -Input.mousePosition.y;
                if (lastFrameMousePos != EasyInputConstants.NOT_TOUCHING)
                {
                    delta = currentMousePos - lastFrameMousePos;
                }
                else
                {
                    delta = EasyInputConstants.NOT_TOUCHING;
                }
            }
            else
            {
                isController = false;
                isCamera = false;
                currentMousePos = EasyInputConstants.NOT_TOUCHING;
                delta = EasyInputConstants.NOT_TOUCHING;
            }

            if (delta != EasyInputConstants.NOT_TOUCHING && delta != Vector2.zero)
            {
                if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftAlt))
                {
                    OrientationDelta.z = 0f;
                    OrientationDelta.y = delta.x;
                    OrientationDelta.x = delta.y;
                }
                else if (Input.GetKey(KeyCode.RightControl))
                {
                    OrientationDelta.z = -delta.x;
                    OrientationDelta.y = 0f;
                    OrientationDelta.x = 0f;
                }
            }
            else
            {
                OrientationDelta = Vector3.zero;
            }

            if (isCamera)
            {
                Camera.main.transform.localEulerAngles += OrientationDelta;
            }

            if (isController)
            {
                myMotion.currentOrientationEuler += OrientationDelta;
                myMotion.currentOrientation = Quaternion.Euler(myMotion.currentOrientationEuler);
            }


            myMotion.currentPos = new Vector3(.5f, -.2f, .5f);

            myMotion.currentOrientationEuler = myMotion.currentOrientation.eulerAngles;
            myMotion.currentAngVel = Vector3.zero;
            myMotion.currentAcc = Vector3.zero;

            lastFrameMousePos = currentMousePos;
#endif

#if !UNITY_EDITOR && UNITY_ANDROID
            //real data
            if (isGearVR)
            {
                myMotion.currentOrientation = OVRInput.GetLocalControllerRotation(OVRInput.GetActiveController());
                myMotion.currentOrientationEuler = myMotion.currentOrientation.eulerAngles;
                myMotion.currentAngVel = OVRInput.GetLocalControllerAngularVelocity(OVRInput.GetActiveController());
                myMotion.currentPos = OVRInput.GetLocalControllerPosition(OVRInput.GetActiveController());
                myMotion.currentAcc = OVRInput.GetLocalControllerAcceleration(OVRInput.GetActiveController());
            }
#endif

            //derived data section

            //filter out noisy accel values that are close to zero
            temp = myMotion.currentAcc;
            temp.x = (temp.x <= .15f && temp.x >= -.2f) ? 0f : temp.x;
            temp.y = (temp.y <= .3f && temp.y >= -.35f) ? 0f : temp.y;
            temp.z = (temp.z <= .15f && temp.z >= -.2f) ? 0f : temp.z;

            //another filter for the largest of magnitudes which is usually the stop of a motion
            temp.x = (temp.x >= 5f) ? 0f : temp.x;
            temp.y = (temp.y >= 5f) ? 0f : temp.y;
            temp.z = (temp.z >= 5f) ? 0f : temp.z;

            myMotion.currentVel = (temp * Time.deltaTime) + myMotion.lastFrameVel;

            //can still have propogated velocity that we need to filter out
            framesVelocitySameX = (myMotion.currentVel.x == myMotion.lastFrameVel.x) ? (framesVelocitySameX+1) : 0;
            framesVelocitySameY = (myMotion.currentVel.y == myMotion.lastFrameVel.y) ? (framesVelocitySameY+1) : 0;
            framesVelocitySameZ = (myMotion.currentVel.z == myMotion.lastFrameVel.z) ? (framesVelocitySameZ+1) : 0;



            if (framesVelocitySameX >= 2)
            {
                myMotion.lastFrameVel.x = 0f;
                myMotion.currentVel.x = 0f;
                framesVelocitySameX = 0;
            }
            if (framesVelocitySameY >= 2)
            {
                myMotion.lastFrameVel.y = 0f;
                myMotion.currentVel.y = 0f;
                framesVelocitySameY = 0;
            }
            if (framesVelocitySameZ >= 2)
            {
                myMotion.lastFrameVel.z = 0f;
                myMotion.currentVel.z = 0f;
                framesVelocitySameZ = 0;
            }


            myMotion.posSinceLastReset += (myMotion.currentVel * Time.deltaTime);
            myMotion.totalVelSinceLastReset += myMotion.currentVel;
            myMotion.totalAngularVelSinceLastReset += myMotion.currentAngVel;

            //fire off the delegate
            if (On_Motion != null)
                On_Motion(myMotion);
        }

        void processSpecificButton(ref ButtonClick buttonClick)
        {
            buttonClick.currentlyPressed = getControllerButtonState(buttonClick.button);

            if (buttonClick.currentlyPressed)
            {

                if (!buttonClick.previousFramePressed)
                {
                    //start of a click
                    buttonClick.currentClickBeginTimestamp = Time.time;

                    //touchtype starts at quick if new or double if were in the time factor for double press
                    if (buttonClick.previousClickBeginTimestamp != EasyInputConstants.NO_TIMESTAMP
                        && (buttonClick.currentClickBeginTimestamp - buttonClick.previousClickBeginTimestamp) < maxDoubleTapTime)
                    {
                        //possible start of a double tap
                        buttonClick.clickType = EasyInputConstants.CLICK_TYPE.DoublePress;
                    }
                    else
                    {
                        buttonClick.clickType = EasyInputConstants.CLICK_TYPE.QuickPress;
                    }

                    //fire off the click start event and touch event
                    if (On_ClickStart != null)
                        On_ClickStart(buttonClick);
                    if (On_Click != null)
                        On_Click(buttonClick);

                }
                else
                {
                    //continue of a press

                    //change to quickpress
                    if (buttonClick.clickType == EasyInputConstants.CLICK_TYPE.Miscellaneous
                        && Time.time - buttonClick.currentClickBeginTimestamp < maxQuickTapTime)
                    {
                        buttonClick.clickType = EasyInputConstants.CLICK_TYPE.QuickPress;
                    }
                    //change to longpress
                    if (buttonClick.clickType != EasyInputConstants.CLICK_TYPE.LongPress
                        && Time.time - buttonClick.currentClickBeginTimestamp > minLongTapTime)
                    {
                        buttonClick.clickType = EasyInputConstants.CLICK_TYPE.LongPress;
                        if (On_LongClickStart != null)
                            On_LongClickStart(buttonClick);
                    }

                    //change to miscellaneous
                    if (buttonClick.clickType != EasyInputConstants.CLICK_TYPE.Miscellaneous)
                    {
                        //downgraded from quickpress
                        if (buttonClick.clickType == EasyInputConstants.CLICK_TYPE.QuickPress)
                        {
                            if (Time.time - buttonClick.currentClickBeginTimestamp > maxQuickTapTime)
                            {
                                //we were a quick press but now we've taken too long revert to misc
                                buttonClick.clickType = EasyInputConstants.CLICK_TYPE.Miscellaneous;
                            }
                        }

                        //downgraded from double press
                        else if (buttonClick.clickType == EasyInputConstants.CLICK_TYPE.DoublePress)
                        {
                            if (Time.time - buttonClick.currentClickBeginTimestamp > maxDoubleTapTime)
                            {
                                //we were a double press but now we've taken too long revert to misc
                                buttonClick.clickType = EasyInputConstants.CLICK_TYPE.Miscellaneous;
                            }
                        }
                    }



                    //if touching we always fire off the touch event
                    if (On_Click != null)
                        On_Click(buttonClick);

                    //if touching and currently a long press always fire that too
                    if (buttonClick.clickType == EasyInputConstants.CLICK_TYPE.LongPress)
                    {
                        if (On_LongClick != null)
                            On_LongClick(buttonClick);
                    }

                }
                buttonClick.previousFramePressed = true;

            }
            else
            {
                //no current touch 

                //first look at previous values to determine if we need to do anything
                if (buttonClick.previousFramePressed == true)
                {
                    //we just ended a touch
                    if (On_ClickEnd != null)
                        On_ClickEnd(buttonClick);

                    //also fire end of quick or long if it was this (remember we don't fire a swipe event on end we fire it
                    //on each time we hit the swipe length
                    if (buttonClick.clickType == EasyInputConstants.CLICK_TYPE.QuickPress)
                    {
                        //double check that we've just ended a quick touch
                        if (Time.time - buttonClick.currentClickBeginTimestamp < maxQuickTapTime)
                            if (On_QuickClickEnd != null)
                                On_QuickClickEnd(buttonClick);
                    }
                    else if (buttonClick.clickType == EasyInputConstants.CLICK_TYPE.LongPress)
                    {
                        //double check that we've just ended a long touch
                        if (Time.time - buttonClick.currentClickBeginTimestamp > minLongTapTime)
                            if (On_LongClickEnd != null)
                                On_LongClickEnd(buttonClick);
                    }
                    else if (buttonClick.clickType == EasyInputConstants.CLICK_TYPE.DoublePress)
                    {
                        //double check that we've just ended a double touch
                        if (Time.time - buttonClick.previousClickBeginTimestamp < maxDoubleTapTime)
                            if (On_DoubleClickEnd != null)
                                On_DoubleClickEnd(buttonClick);
                    }

                    //since we were previously touching set the timestamp
                    buttonClick.previousClickBeginTimestamp = buttonClick.currentClickBeginTimestamp;
                }


                //reset the variables since there is no current touch
                buttonClick.currentClickBeginTimestamp = EasyInputConstants.NO_TIMESTAMP;
                buttonClick.clickType = EasyInputConstants.CLICK_TYPE.Miscellaneous;
                buttonClick.previousFramePressed = false;
            }
        }

        void processSpecificAxis(ref ControllerAxis axis)
        {

            switch (axis.axis)
            {
                case EasyInputConstants.CONTROLLER_AXIS.LeftStick:
                    axis.axisValue.x = Input.GetAxisRaw(EasyInputConstants.P1_LEFTSTICK_HORIZONTAL);
                    axis.axisValue.y = Input.GetAxisRaw(EasyInputConstants.P1_LEFTSTICK_VERTICAL);
                    if (On_LeftStick != null)
                        On_LeftStick(axis);
                    break;
                case EasyInputConstants.CONTROLLER_AXIS.RightStick:
                    axis.axisValue.x = Input.GetAxisRaw(EasyInputConstants.P1_RIGHTSTICK_HORIZONTAL);
                    axis.axisValue.y = Input.GetAxisRaw(EasyInputConstants.P1_RIGHTSTICK_VERTICAL);
                    if (On_RightStick != null)
                        On_RightStick(axis);
                    break;
                case EasyInputConstants.CONTROLLER_AXIS.DPad:
                    axis.axisValue.x = Input.GetAxisRaw(EasyInputConstants.P1_DPAD_HORIZONTAL);
                    axis.axisValue.y = Input.GetAxisRaw(EasyInputConstants.P1_DPAD_VERTICAL);
                    if (On_Dpad != null)
                        On_Dpad(axis);
                    break;
                case EasyInputConstants.CONTROLLER_AXIS.LeftTrigger:
                    axis.axisValue.x = Input.GetAxisRaw(EasyInputConstants.P1_LEFTTRIGGER);
                    axis.axisValue.y = Input.GetAxisRaw(EasyInputConstants.P1_LEFTTRIGGER);
                    if (On_LeftTrigger != null)
                        On_LeftTrigger(axis);
                    break;
                case EasyInputConstants.CONTROLLER_AXIS.RightTrigger:
                    axis.axisValue.x = Input.GetAxisRaw(EasyInputConstants.P1_RIGHTTRIGGER);
                    axis.axisValue.y = Input.GetAxisRaw(EasyInputConstants.P1_RIGHTTRIGGER);
                    if (On_RightTrigger != null)
                        On_RightTrigger(axis);
                    break;
                default:
                    break;
            }



            
        }

        bool getControllerButtonState(EasyInputConstants.CONTROLLER_BUTTON button)
        {
#if UNITY_EDITOR
            switch (button)
            {
                case EasyInputConstants.CONTROLLER_BUTTON.AButton:
                    return (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Return));
                case EasyInputConstants.CONTROLLER_BUTTON.BButton:
                    return Input.GetKey(KeyCode.B);
                case EasyInputConstants.CONTROLLER_BUTTON.XButton:
                    return Input.GetKey(KeyCode.X);
                case EasyInputConstants.CONTROLLER_BUTTON.YButton:
                    return Input.GetKey(KeyCode.Y);
                case EasyInputConstants.CONTROLLER_BUTTON.LeftBumper:
                    return Input.GetKey(KeyCode.L);
                case EasyInputConstants.CONTROLLER_BUTTON.RightBumper:
                    return Input.GetKey(KeyCode.R);
                case EasyInputConstants.CONTROLLER_BUTTON.StartButton:
                    return Input.GetKey(KeyCode.S);
                case EasyInputConstants.CONTROLLER_BUTTON.Back:
                    return Input.GetKey(KeyCode.B);
                case EasyInputConstants.CONTROLLER_BUTTON.LeftStickPush:
                    return Input.GetKey(KeyCode.C);
                case EasyInputConstants.CONTROLLER_BUTTON.RightStickPush:
                    return Input.GetKey(KeyCode.V);
                case EasyInputConstants.CONTROLLER_BUTTON.GearVRTouchClick:
                    return Input.GetKey(KeyCode.T);
                case EasyInputConstants.CONTROLLER_BUTTON.GearVRTrigger:
                    return Input.GetKey(KeyCode.U);
                case EasyInputConstants.CONTROLLER_BUTTON.GearVRHMDPadTap:
                    return Input.GetKey(KeyCode.Q);
                default:
                    return false;
            }

#endif


#if !UNITY_EDITOR && UNITY_ANDROID
            switch (button)
            {
                case EasyInputConstants.CONTROLLER_BUTTON.AButton:
                    return Input.GetKey(KeyCode.Joystick1Button0);
                case EasyInputConstants.CONTROLLER_BUTTON.BButton:
                    return Input.GetKey(KeyCode.Joystick1Button1);
                case EasyInputConstants.CONTROLLER_BUTTON.XButton:
                    return Input.GetKey(KeyCode.Joystick1Button2);
                case EasyInputConstants.CONTROLLER_BUTTON.YButton:
                    return Input.GetKey(KeyCode.Joystick1Button3);
                case EasyInputConstants.CONTROLLER_BUTTON.LeftBumper:
                    return Input.GetKey(KeyCode.Joystick1Button4);
                case EasyInputConstants.CONTROLLER_BUTTON.RightBumper:
                    return Input.GetKey(KeyCode.Joystick1Button5);
                case EasyInputConstants.CONTROLLER_BUTTON.StartButton: //start button
                    return Input.GetKey(KeyCode.Joystick1Button10);
                case EasyInputConstants.CONTROLLER_BUTTON.Back:
                    return (Input.GetKey(KeyCode.Joystick1Button11)|| Input.GetKey(KeyCode.Escape));
                case EasyInputConstants.CONTROLLER_BUTTON.LeftStickPush:
                    return Input.GetKey(KeyCode.Joystick1Button8);
                case EasyInputConstants.CONTROLLER_BUTTON.RightStickPush:
                    return Input.GetKey(KeyCode.Joystick1Button9);
                case EasyInputConstants.CONTROLLER_BUTTON.GearVRTouchClick:
                    if (isGearVR)
                    {
                        return OVRInput.Get(OVRInput.Button.PrimaryTouchpad);
                    }
                    return false;
                case EasyInputConstants.CONTROLLER_BUTTON.GearVRTrigger:
                    if (isGearVR)
                    {
                        return OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
                    }
                    return false;
                case EasyInputConstants.CONTROLLER_BUTTON.GearVRHMDPadTap:
                    if (isGearVR)
                    {
                        return OVRInput.Get(OVRInput.Touch.PrimaryTouchpad) && (!(OVRInput.GetActiveController() == OVRInput.Controller.LTrackedRemote || OVRInput.GetActiveController() == OVRInput.Controller.RTrackedRemote));
                    }
                    return false;
                default:
                    return false;
            }

#endif
#if !UNITY_EDITOR && !UNITY_ANDROID
            return false;
#endif

        }
    }

}
