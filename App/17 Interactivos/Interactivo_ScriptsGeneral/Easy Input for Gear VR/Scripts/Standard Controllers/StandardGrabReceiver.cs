using UnityEngine;
using System.Collections;
using EasyInputVR.Core;
using UnityEngine.Events;

namespace EasyInputVR.StandardControllers
{

    [AddComponentMenu("EasyInputGearVR/Standard Controllers/StandardGrabReceiver")]
    public class StandardGrabReceiver : StandardBaseReceiver
    {

        public EasyInputConstants.BUTTON_CONDITION grabCondtion = EasyInputConstants.BUTTON_CONDITION.GearVRTriggerPressed;
        public EasyInputConstants.DROP_MODE dropCondition = EasyInputConstants.DROP_MODE.Drop;
        public float throwSensitivity = 1f;
        public bool blockMovementWhenGrabbing;
        public StandardBaseMovement movementObject;
        public bool forceDepthToDistance;
        public float forcedDistance = 1f;
        public bool allowClickForDepth;
        public float clickDeadzone = .4f;
        public float clickSensitivity = 1f;
        public float minDepthDistance = .5f;
        public float maxDepthDistance = 20f;
        public bool allowSwipeForRotation;
        public float swipeXSensitivity = 1f;
        public float swipeYSensitivity = 1f;
        public bool collisionsBreakGrab;
        public bool lockXPosition;
        public bool lockYPosition;
        public bool lockZPosition;
        public bool lockXRotation;
        public bool lockYRotation;
        public bool lockZRotation;
        public EasyInputConstants.ROTATION_MODE twistMode = EasyInputConstants.ROTATION_MODE.None;
        public float twistSensitivity = 1f;

        Rigidbody myRigidbody;
        bool clicking;
        bool padClick;
        bool hovering;
        bool grabMode;
        float currentDistance;
        bool previousClicking;
        Vector3 initialPosition;
        Vector3 temp;
        Quaternion initialRotation;
        Vector2 lastTouch;
        float clickDelta;
        Vector2 swipeDelta;
        float initialTwist;
        Transform laserTransform;
        Vector3 lastFramePosition;
        Vector3 twoFramesAgoPosition;
        Vector3 threeFramesAgoPosition;
        Vector3 lastHitPosition;
        Vector3 initialOffset;



        void OnEnable()
        {
            EasyInputHelper.On_Touch += localTouch;
            EasyInputHelper.On_TouchEnd += localTouchEnd;
            EasyInputHelper.On_TouchStart += localTouchStart;
            EasyInputHelper.On_ClickStart += clickStart;
            EasyInputHelper.On_ClickEnd += clickEnd;

            myRigidbody = this.GetComponent<Rigidbody>();
        }

        void OnDestroy()
        {
            EasyInputHelper.On_Touch -= localTouch;
            EasyInputHelper.On_TouchEnd -= localTouchEnd;
            EasyInputHelper.On_TouchStart -= localTouchStart;
            EasyInputHelper.On_ClickStart -= clickStart;
            EasyInputHelper.On_ClickEnd -= clickEnd;

        }


        void FixedUpdate()
        {
            if (grabMode)
            {
                if (padClick)
                {
                    currentDistance += clickDelta;
                    if (currentDistance > maxDepthDistance)
                        currentDistance = maxDepthDistance;
                    else if (currentDistance < minDepthDistance)
                        currentDistance = minDepthDistance;
                }

                //check if we are forcing a distance
                if (forceDepthToDistance)
                    currentDistance = forcedDistance;

                this.transform.position = (laserTransform.position + laserTransform.forward * currentDistance) + initialOffset;

                //we're grabbing the object so we don't want gravity or other objects to move or spin the object
                if (myRigidbody != null)
                {
                    myRigidbody.velocity = Vector3.zero;
                    myRigidbody.angularVelocity = Vector3.zero;
                }

                //deal with the locks from the inspector for position
                temp = this.transform.position;

                if (lockXPosition)
                    temp.x = initialPosition.x + initialOffset.x;
                if (lockYPosition)
                    temp.y = initialPosition.y + initialOffset.y;
                if (lockZPosition)
                    temp.z = initialPosition.z +initialOffset.z;

                this.transform.position = temp;

                //deal with the rotation mode (twist is local rotation)
                temp = this.transform.localRotation.eulerAngles;
                if (twistMode == EasyInputConstants.ROTATION_MODE.TwistLocalXAxis)
                    temp.x = (initialTwist - laserTransform.localRotation.eulerAngles.z) * twistSensitivity;
                if (twistMode == EasyInputConstants.ROTATION_MODE.TwistLocalYAxis)
                    temp.y = (initialTwist - laserTransform.localRotation.eulerAngles.z) * twistSensitivity;
                if (twistMode == EasyInputConstants.ROTATION_MODE.TwistLocalZAxis)
                    temp.z = (initialTwist - laserTransform.localRotation.eulerAngles.z) * twistSensitivity;

                this.transform.localRotation = Quaternion.Euler(temp);

                //deal with the swipes
                temp = this.transform.localRotation.eulerAngles;
                if (allowSwipeForRotation && swipeDelta != Vector2.zero)
                {
                    //horizontal on the pad is yaw
                    temp.y += swipeDelta.x * swipeXSensitivity * 100f;
                    //vertical on the pad is pitch
                    temp.x += swipeDelta.y * swipeYSensitivity * 100f;
                }
                this.transform.localRotation = Quaternion.Euler(temp);

                //deal with the locks from the inspector for rotation
                temp = this.transform.localRotation.eulerAngles;
                if (lockXRotation)
                    temp.x = initialRotation.eulerAngles.x;
                if (lockYRotation)
                    temp.y = initialRotation.eulerAngles.y;
                if (lockZRotation)
                    temp.z = initialRotation.eulerAngles.z;

                this.transform.localRotation = Quaternion.Euler(temp);

                


            }
        }

        void Update()
        {
            //set this for next frame
            threeFramesAgoPosition = twoFramesAgoPosition;
            twoFramesAgoPosition = lastFramePosition;
            lastFramePosition = this.transform.position;
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collisionsBreakGrab)
            {
                clicking = false;
                previousClicking = false;
                grabMode = false;
                if (myRigidbody != null)
                {
                    myRigidbody.velocity = Vector3.zero;
                    myRigidbody.angularVelocity = Vector3.zero;
                }
            }

        }

        public override void Hover(Vector3 hitPosition, Transform pointerTransform)
        {
            lastHitPosition = hitPosition;
            hovering = true;
            if (clicking)
            {
                grabMode = true;
                laserTransform = pointerTransform;
                if (!previousClicking)
                {
                    //first frame of grab get the distance
                    currentDistance = (laserTransform.position - hitPosition).magnitude;
                    previousClicking = true;
                }
            }
        }

        public override void HoverEnter(Vector3 hitPosition, Transform pointerTransform)
        {
        }

        public override void HoverExit(Vector3 hitPosition, Transform pointerTransform)
        {
            hovering = false;
        }

        void localTouchStart(InputTouch touch)
        {
            lastTouch = touch.currentTouchPosition;
            swipeDelta = Vector2.zero;
        }

        void localTouch(InputTouch touch)
        {
            if (allowSwipeForRotation)
            {
                swipeDelta = (touch.currentTouchPosition - lastTouch);
                lastTouch = touch.currentTouchPosition;

                if (grabMode && blockMovementWhenGrabbing && movementObject != null)
                    movementObject.blockMovement();
            }

            if (padClick && allowClickForDepth)
            {
                //clicking up or down on the dpad moves the object closer/further
                clickDelta = 0f;

                if (Mathf.Sqrt(Mathf.Pow(touch.currentTouchPosition.x, 2) + Mathf.Pow(touch.currentTouchPosition.y, 2)) > clickDeadzone)
                {
                    //we have a valid touch
                    //only two directions (up/down)
                    if (Mathf.Abs(touch.currentTouchPosition.y) >= Mathf.Abs(touch.currentTouchPosition.x))
                    {
                        clickDelta = (touch.currentTouchPosition.y > 0f) ? clickSensitivity : -clickSensitivity;
                    }
                }

            }
        }

        void localTouchEnd(InputTouch touch)
        {
            swipeDelta = Vector2.zero;
            clickDelta = 0f;

            if (blockMovementWhenGrabbing && movementObject != null)
                movementObject.unblockMovement();
        }

        void clickStart(ButtonClick button)
        {
            if ((int)button.button == (int)grabCondtion && hovering)
            {
                clicking = true;
                initialOffset = this.transform.position - lastHitPosition;
                initialPosition = this.transform.position;
                initialRotation = this.transform.localRotation;
                if (laserTransform != null)
                    initialTwist = laserTransform.localRotation.eulerAngles.z;
            }

            if (button.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTouchClick)
            {
                padClick = true;
                clickDelta = 0f;
            }
        }

        void clickEnd(ButtonClick button)
        {
            if ((int)button.button == (int)grabCondtion && grabMode)
            {
                clicking = false;
                previousClicking = false;
                grabMode = false;
                if (myRigidbody != null)
                {
                    if (dropCondition == EasyInputConstants.DROP_MODE.Drop)
                    {
                        myRigidbody.velocity = Vector3.zero;
                        myRigidbody.angularVelocity = Vector3.zero;
                    }
                    else if (dropCondition == EasyInputConstants.DROP_MODE.ThrowLatestVelocity)
                    {
                        myRigidbody.velocity = ((lastFramePosition - twoFramesAgoPosition) / Time.deltaTime) * throwSensitivity;
                        myRigidbody.angularVelocity = Vector3.zero;

                    }
                    else if (dropCondition == EasyInputConstants.DROP_MODE.ThrowAvgLastTwoFrames)
                    {
                        myRigidbody.velocity = ((lastFramePosition - threeFramesAgoPosition) / (2*Time.deltaTime)) * throwSensitivity;
                        myRigidbody.angularVelocity = Vector3.zero;

                    }
                }
            }

            if (button.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTouchClick)
            {
                padClick = false;
                clickDelta = 0f;
            }
        }

    }

}
