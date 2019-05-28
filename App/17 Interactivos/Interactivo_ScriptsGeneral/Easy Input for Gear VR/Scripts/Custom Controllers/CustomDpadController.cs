using UnityEngine;
using System.Collections;
using EasyInputVR.Core;
using System;
using UnityEngine.Events;

namespace EasyInputVR.CustomControllers
{

    [AddComponentMenu("EasyInputGearVR/Custom Controllers/CustomDpadController")]
    public class CustomDpadController : MonoBehaviour
    {
        [Range(0f, 1f)]
        public float deadZone = .4f;
        public EasyInputConstants.DIRECTION_MODE directionMode = EasyInputConstants.DIRECTION_MODE.FourDirectionsOnly;
        public EasyInputConstants.REGISTER_MODE dpadMode = EasyInputConstants.REGISTER_MODE.RegisterAlways;

        //events
        [System.Serializable]
        public class TouchHandler : UnityEvent<InputTouch> { }
        [SerializeField]
        public TouchHandler onLeft;
        [SerializeField]
        public TouchHandler onRight;
        [SerializeField]
        public TouchHandler onUp;
        [SerializeField]
        public TouchHandler onDown;


        //runtime variables
        bool isClicking = false;

        void OnEnable()
        {
            EasyInputHelper.On_Touch += localAxis;

            if (dpadMode == EasyInputConstants.REGISTER_MODE.RegisterOnlyWhenClicking)
            {
                EasyInputHelper.On_ClickStart += localClickStart;
                EasyInputHelper.On_ClickEnd += localClickEnd;
            }
        }

        void OnDestroy()
        {
            EasyInputHelper.On_Touch -= localAxis;

            if (dpadMode == EasyInputConstants.REGISTER_MODE.RegisterOnlyWhenClicking)
            {
                EasyInputHelper.On_ClickStart -= localClickStart;
                EasyInputHelper.On_ClickEnd -= localClickEnd;
            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        void localAxis(InputTouch touch)
        {
            if (dpadMode == EasyInputConstants.REGISTER_MODE.RegisterAlways || isClicking)
            {

                //check to see if we've exceeded the deadzone
                if (Mathf.Sqrt(Mathf.Pow(touch.currentTouchPosition.x, 2) + Mathf.Pow(touch.currentTouchPosition.y, 2)) > deadZone)
                {
                    //we have a valid touch
                    if (directionMode == EasyInputConstants.DIRECTION_MODE.FourDirectionsOnly)
                    {
                        //only four directions (up/down/left/right
                        if (Mathf.Abs(touch.currentTouchPosition.x) >= Mathf.Abs(touch.currentTouchPosition.y))
                        {
                            //horizontal is greater than vertical it's either left or right
                            if (touch.currentTouchPosition.x > 0f)
                            {
                                //right
                                onRight.Invoke(touch);
                            }
                            else
                            {
                                //left
                                onLeft.Invoke(touch);
                            }
                        }
                        else
                        {
                            //vertical is greater than horizontal it's either up or down
                            if (touch.currentTouchPosition.y > 0f)
                            {
                                //up
                                onUp.Invoke(touch);
                            }
                            else
                            {
                                //down
                                onDown.Invoke(touch);
                            }
                        }
                    }
                    else
                    {
                        //eight possible directions (up/upleft/upright/left/right/down/downleft/downright)
                        //only four directions (up/down/left/right
                        if (Mathf.Abs(touch.currentTouchPosition.x) >= Mathf.Abs(touch.currentTouchPosition.y))
                        {
                            //horizontal is greater than vertical
                            if (touch.currentTouchPosition.x > 0f)
                            {
                                //it's either right/rightup/rightdown
                                if (Mathf.Abs(touch.currentTouchPosition.x) >= (2f * Mathf.Abs(touch.currentTouchPosition.y)))
                                {
                                    onRight.Invoke(touch);
                                }
                                else if (touch.currentTouchPosition.y > 0f)
                                {
                                    onRight.Invoke(touch);
                                    onUp.Invoke(touch);
                                }
                                else
                                {
                                    onRight.Invoke(touch);
                                    onDown.Invoke(touch);
                                }
                            }
                            else
                            {
                                //it's either left/leftup/leftdown
                                if (Mathf.Abs(touch.currentTouchPosition.x) >= (2f * Mathf.Abs(touch.currentTouchPosition.y)))
                                {
                                    onLeft.Invoke(touch);
                                }
                                else if (touch.currentTouchPosition.y > 0f)
                                {
                                    onLeft.Invoke(touch);
                                    onUp.Invoke(touch);
                                }
                                else
                                {
                                    onLeft.Invoke(touch);
                                    onDown.Invoke(touch);
                                }
                            }
                        }
                        else
                        {
                            //vertical is greater than horizontal
                            if (touch.currentTouchPosition.y > 0f)
                            {
                                //it's either up/rightup/leftup
                                if (Mathf.Abs(touch.currentTouchPosition.y) >= (2f * Mathf.Abs(touch.currentTouchPosition.x)))
                                {
                                    onUp.Invoke(touch);
                                }
                                else if (touch.currentTouchPosition.x > 0f)
                                {
                                    onRight.Invoke(touch);
                                    onUp.Invoke(touch);
                                }
                                else
                                {
                                    onLeft.Invoke(touch);
                                    onUp.Invoke(touch);
                                }
                            }
                            else
                            {
                                //it's either down/leftdown/rightdown
                                if (Mathf.Abs(touch.currentTouchPosition.y) >= (2f * Mathf.Abs(touch.currentTouchPosition.x)))
                                {
                                    onDown.Invoke(touch);
                                }
                                else if (touch.currentTouchPosition.x > 0f)
                                {
                                    onRight.Invoke(touch);
                                    onDown.Invoke(touch);
                                }
                                else
                                {
                                    onLeft.Invoke(touch);
                                    onDown.Invoke(touch);
                                }
                            }
                        }
                    }
                    
                
                }


                
            }
        }

        void localClickStart(ButtonClick click)
        {
            if (click.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTouchClick)
            {
                isClicking = true;
            }
        }

        void localClickEnd(ButtonClick click)
        {
            if (click.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTouchClick)
            {
                isClicking = false;
            }
        }


    }

}

