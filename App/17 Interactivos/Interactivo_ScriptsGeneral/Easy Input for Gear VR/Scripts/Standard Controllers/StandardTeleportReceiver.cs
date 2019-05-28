using UnityEngine;
using System.Collections;
using EasyInputVR.Core;
using UnityEngine.Events;
using System;

namespace EasyInputVR.StandardControllers
{

    [AddComponentMenu("EasyInputGearVR/Standard Controllers/StandardTeleportReceiver")]
    public class StandardTeleportReceiver : StandardBaseReceiver
    {

        public EasyInputConstants.ACTION_CONDITION teleportCondition = EasyInputConstants.ACTION_CONDITION.GearVRTriggerPressed;
        public EasyInputConstants.TELEPORT_MODE teleportMode = EasyInputConstants.TELEPORT_MODE.AlwaysShowLaser;
        public GameObject teleportObject;
        public StandardBaseLaser laser;
        public float yAxisOffset = 0f;
        public float timeLockout = 2f;

        bool fireEvent;
        bool clicking;
        bool stoppedClicking;
        bool stoppedTouching;
        float lastTeleport = 0f;
        Vector2 touchPos;
        Vector3 lastHitPosition;


        void OnEnable()
        {


            if (teleportCondition == EasyInputConstants.ACTION_CONDITION.TouchpadTouched)
            {
                EasyInputHelper.On_Touch += localTouch;
                EasyInputHelper.On_TouchEnd += localTouchEnd;
            }
            else
            {
                EasyInputHelper.On_ClickEnd += clickEnd;
                EasyInputHelper.On_Click += click;
            }

        }

        void OnDestroy()
        {
            if (teleportCondition == EasyInputConstants.ACTION_CONDITION.TouchpadTouched)
            {
                EasyInputHelper.On_Touch -= localTouch;
                EasyInputHelper.On_TouchEnd -= localTouchEnd;
            }
            else
            {
                EasyInputHelper.On_ClickEnd -= clickEnd;
                EasyInputHelper.On_Click -= click;
            }
        }


        void Update()
        {
            if (teleportMode == EasyInputConstants.TELEPORT_MODE.ShowLaserOnConditionStart)
            {
                if (teleportCondition == EasyInputConstants.ACTION_CONDITION.TouchpadTouched)
                {
                    if (touchPos == EasyInputConstants.NOT_TOUCHING)
                    {
                        //not touching no laser
                        laser.TurnOffLaserAndReticle();
                    }
                    else
                    {
                        laser.TurnOnLaserAndReticle();
                    }

                    //check to see if we just stopped
                    if (stoppedTouching && lastHitPosition != EasyInputConstants.NOT_VALID && (Time.time - lastTeleport) > timeLockout)
                    {
                        //teleport
                        lastTeleport = Time.time;
                        lastHitPosition.y += yAxisOffset;
                        teleportObject.transform.position = lastHitPosition;
                    }
                }
                else
                {
                    if (!clicking)
                    {
                        //not clicking no laser
                        laser.TurnOffLaserAndReticle();
                    }
                    else
                    {
                        laser.TurnOnLaserAndReticle();
                    }

                    //check to see if we just stopped
                    if (stoppedClicking && lastHitPosition != EasyInputConstants.NOT_VALID && (Time.time - lastTeleport) > timeLockout)
                    {
                        //teleport
                        lastTeleport = Time.time;
                        lastHitPosition.y += yAxisOffset;
                        teleportObject.transform.position = lastHitPosition;
                    }
                }
            }



            //done processing reset variable
            stoppedClicking = false;
            stoppedTouching = false;
            lastHitPosition = EasyInputConstants.NOT_VALID;
        }


        public override void Hover(Vector3 hitPosition, Transform pointerTransform)
        {
            lastHitPosition = hitPosition;
            if (teleportMode == EasyInputConstants.TELEPORT_MODE.AlwaysShowLaser)
            {
                //if we're always showing the laser we teleport on button down
                if (conditionsMet() && (Time.time - lastTeleport) > timeLockout)
                {
                    //teleport
                    lastTeleport = Time.time;
                    hitPosition.y += yAxisOffset;
                    teleportObject.transform.position = hitPosition;
                }
            }
        }

        public override void HoverEnter(Vector3 hitPosition, Transform pointerTransform)
        {

        }

        public override void HoverExit(Vector3 hitPosition, Transform pointerTransform)
        {
        }

        void localTouch(InputTouch touch)
        {
            touchPos = touch.currentTouchPosition;
        }

        void localTouchEnd(InputTouch touch)
        {
            touchPos = EasyInputConstants.NOT_TOUCHING;
            stoppedTouching = true;
        }


        void clickEnd(ButtonClick button)
        {
            if ((int)button.button == (int)teleportCondition)
            {
                clicking = false;
                stoppedClicking = true;
            }
        }

        void click(ButtonClick button)
        {
            if ((int)button.button == (int)teleportCondition)
                clicking = true;
        }

        bool conditionsMet()
        {
            fireEvent = false;
            if (teleportCondition == EasyInputConstants.ACTION_CONDITION.TouchpadTouched && !touchPos.Equals(EasyInputConstants.NOT_TOUCHING))
                fireEvent = true;
            else if (clicking)
                fireEvent = true;


            return fireEvent;


        }
    }

}
