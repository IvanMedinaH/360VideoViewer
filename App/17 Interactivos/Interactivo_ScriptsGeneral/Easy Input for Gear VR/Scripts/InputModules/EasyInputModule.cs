using System;
using EasyInputVR.Core;

namespace UnityEngine.EventSystems
{
    [AddComponentMenu("EasyInputGearVR/Input Modules/Easy Input Module")]
    public class EasyInputModule : PointerInputModule
    {

        protected EasyInputModule()
        { }
        public float repeatEventRate = .25f;
        public EasyInputConstants.INPUT_MODULE_BUTTON_MODE buttonMode = EasyInputConstants.INPUT_MODULE_BUTTON_MODE.FireAtRepeatRate;
        [Range(.0001f, 1f)]
        public float scrollAmount = .01f;
        [Range(1,100)]
        public int scrollSpeedMultiplier = 3;

        
        
        public Camera UICamera;

        //runtime variables
        Vector2 leftStick;
        Vector2 dpad;
        Vector2 pos;
        Vector2 lastPosition = Vector2.zero;
        InputTouch touchpad = new InputTouch();
        bool submit;
        bool cancel;
        bool submitForOneFrame;
        bool cancelForOneFrame;
        int horizontalStepsFired = 0;
        int verticalStepsFired = 0;
        DateTime lastEvent = DateTime.UtcNow;
        UI.Scrollbar currentScrollbar;
        UI.Slider currentSlider;
        Vector3 rayOrigin;
        Quaternion rayOrientation;
        private PointerEventData eventData;
        bool triggerDownThisFrame = false;
        bool triggerUpThisFrame = false;
        RaycastResult result;
        Vector3 uiHitPosition;
        float uiMaxDistance;



        protected override void OnEnable()
        {
            base.OnEnable();
            EasyInputHelper.On_ClickStart += localClickStart;
            EasyInputHelper.On_ClickEnd += localClickEnd;
            EasyInputHelper.On_LeftStick += localLeftStick;
            EasyInputHelper.On_Dpad += localDpad;
            EasyInputHelper.On_Touch += localTouch;
            EasyInputHelper.On_TouchEnd += localTouchEnd;
        }

        protected override void OnDestroy()
        {
            EasyInputHelper.On_ClickStart -= localClickStart;
            EasyInputHelper.On_ClickEnd -= localClickEnd;
            EasyInputHelper.On_LeftStick -= localLeftStick;
            EasyInputHelper.On_Dpad -= localDpad;
            EasyInputHelper.On_Touch -= localTouch;
            EasyInputHelper.On_TouchEnd -= localTouchEnd;
            base.OnDestroy();
        }

        protected override void Start()
        {

            // Create a new camera that will be used for raycasts
           /* UICamera = new GameObject("UI Camera").AddComponent<Camera>();
            UICamera.clearFlags = CameraClearFlags.Skybox;
           // UICamera.cullingMask = 1 << 5;
            UICamera.fieldOfView = 90;
            UICamera.nearClipPlane = 0.1f;

            // Find canvases in the scene and assign our custom UICamera to them
            Canvas[] canvases = Resources.FindObjectsOfTypeAll<Canvas>();
            foreach (Canvas canvas in canvases)
            {
                canvas.worldCamera = UICamera;
            }*/
        }


        public override void UpdateModule()
        {
        }

        public override bool IsModuleSupported()
        {
            //if we place an easy input module obviously we want to support it
            return true;
        }

        public override bool ShouldActivateModule()
        {
            if (!base.ShouldActivateModule())
                return false;

            var shouldActivate = false;

            //submit
            shouldActivate |= submit;

            //cancel
            shouldActivate |= cancel;

            //leftstick
            shouldActivate |= (leftStick != Vector2.zero);

            //dpad
            shouldActivate |= (dpad != Vector2.zero);

            //touchpad
            shouldActivate |= (touchpad.currentTouchPosition != EasyInputConstants.NOT_TOUCHING);

            return shouldActivate;
        }

        public override void ActivateModule()
        {
            base.ActivateModule();



            var toSelect = eventSystem.currentSelectedGameObject;
            if (toSelect == null)
                toSelect = eventSystem.firstSelectedGameObject;

            eventSystem.SetSelectedGameObject(null, GetBaseEventData());
            eventSystem.SetSelectedGameObject(toSelect, GetBaseEventData());
        }

        public override void DeactivateModule()
        {
            base.DeactivateModule();
            eventSystem.SetSelectedGameObject(null, GetBaseEventData());
        }

        public override void Process()
        {
            bool usedEvent = SendUpdateEventToSelectedObject();

            if (eventSystem.sendNavigationEvents)
            {
                //we'd use this in a swiping type environment but that doesn't really work in VR
                //if (!usedEvent && !useRaycast)
                //    usedEvent |= SendTouchEventToSelectedObject();

                if (!usedEvent)
                    usedEvent |= SendMoveEventToSelectedObject();

                if (!usedEvent)
                    SendSubmitEventToSelectedObject();
            }

            ProcessMotionController();
            ProcessMove(eventData);
            ProcessDrag(eventData);

            EventSystem.current.SetSelectedGameObject(null);

        }

        private void ProcessMotionController()
        {

            UICamera.transform.position = rayOrigin;
            UICamera.transform.rotation = rayOrientation;

            if (eventData == null)
                eventData = new PointerEventData(eventSystem);
            else
                eventData.Reset();

            pos = new Vector2(UICamera.pixelWidth * 0.5f, UICamera.pixelHeight * 0.5f);

            lastPosition = eventData.position;

            eventData.position = pos;            

            eventSystem.RaycastAll(eventData, m_RaycastResultCache);
            result = FindFirstRaycast(m_RaycastResultCache);
            m_RaycastResultCache.Clear();

            //get the world position from the raycast result
            if (result.gameObject != null && result.worldPosition == Vector3.zero)
            {                
                result.worldPosition = GetIntersectPosition(UICamera, result);
                uiHitPosition = result.worldPosition;
                if ( (result.worldPosition - UICamera.transform.position).magnitude > uiMaxDistance)
                {
                    //too far away don't regiset hit
                    result.gameObject = null;
                    uiHitPosition = EasyInputConstants.NOT_VALID;
                }
                
            }
            else
            {
                uiHitPosition = EasyInputConstants.NOT_VALID;
            }
            //get the screen position from projecting from the main camera not the UI camera
            eventData.position = Camera.main.WorldToScreenPoint(result.worldPosition);
            eventData.delta = eventData.position - lastPosition;

            eventData.pointerCurrentRaycast = result;

            ProcessPointer();


        }

        private Vector3 GetIntersectPosition(Camera cam, RaycastResult raycastResult)
        {
            float intersectionDistance = raycastResult.distance + cam.nearClipPlane;
            Vector3 intersectPosition = cam.transform.position + cam.transform.forward * intersectionDistance;

            return intersectPosition;
        }

        /// <summary>
        /// Process submit keys.
        /// </summary>
        private bool SendSubmitEventToSelectedObject()
        {
            if (eventSystem.currentSelectedGameObject == null)
                return false;
            if ((DateTime.UtcNow - lastEvent).TotalSeconds < repeatEventRate)
                return false;

            var data = GetBaseEventData();
            if (submit)
            {
                lastEvent = DateTime.UtcNow;
                ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.submitHandler);
                if (submitForOneFrame)
                {
                    submit = false;
                    submitForOneFrame = false;
                }
            }

            if (cancel)
            {
                lastEvent = DateTime.UtcNow;
                ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.cancelHandler);
                if (cancelForOneFrame)
                {
                    cancel = false;
                    cancelForOneFrame = false;
                }
            }
            return data.used;
        }

        /// <summary>
        /// Process move navigation (touch)
        /// </summary>
        private bool SendTouchEventToSelectedObject()
        {
            Vector2 movement = Vector2.zero;
            Vector2 movementThreshold = Vector2.zero;

            if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<UI.Scrollbar>() != null)
            {
                currentScrollbar = EventSystem.current.currentSelectedGameObject.GetComponent<UI.Scrollbar>();
                currentSlider = null;
                if (currentScrollbar.direction == UI.Scrollbar.Direction.LeftToRight || currentScrollbar.direction == UI.Scrollbar.Direction.RightToLeft)
                {
                    movementThreshold.x = scrollAmount * 4f ;
                    movementThreshold.y = EasyInputHelper.mInstance.requiredSwipeLength;
                }
                else
                {
                    movementThreshold.x = EasyInputHelper.mInstance.requiredSwipeLength;
                    movementThreshold.y = scrollAmount * 4f;
                }
            }
            else if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<UI.ScrollRect>() != null)
            {
                if (EventSystem.current.currentSelectedGameObject.GetComponent<UI.ScrollRect>().verticalScrollbar != null
                    && EventSystem.current.currentSelectedGameObject.GetComponent<UI.ScrollRect>().horizontalScrollbar != null)
                {
                    //if we have both scrollbars we'd have an issue with controllers. directions would be taken up by scrolling
                    //and no directions would be left to navigate to other UI objects. When this is the case we scroll the
                    //vertical only since it's much more common
                    currentScrollbar = EventSystem.current.currentSelectedGameObject.GetComponent<UI.ScrollRect>().verticalScrollbar;
                }
                else if (EventSystem.current.currentSelectedGameObject.GetComponent<UI.ScrollRect>().verticalScrollbar != null)
                    currentScrollbar = EventSystem.current.currentSelectedGameObject.GetComponent<UI.ScrollRect>().verticalScrollbar;
                else if (EventSystem.current.currentSelectedGameObject.GetComponent<UI.ScrollRect>().horizontalScrollbar != null)
                    currentScrollbar = EventSystem.current.currentSelectedGameObject.GetComponent<UI.ScrollRect>().horizontalScrollbar;
                else
                    currentScrollbar = null;


                currentSlider = null;

                if (currentScrollbar != null)
                {
                    if (currentScrollbar.direction == UI.Scrollbar.Direction.LeftToRight || currentScrollbar.direction == UI.Scrollbar.Direction.RightToLeft)
                    {
                        movementThreshold.x = scrollAmount * 4f;
                        movementThreshold.y = EasyInputHelper.mInstance.requiredSwipeLength;
                    }
                    else
                    {
                        movementThreshold.x = EasyInputHelper.mInstance.requiredSwipeLength;
                        movementThreshold.y = scrollAmount * 4f;
                    }
                }
            }
            else if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<UI.Slider>() != null)
            {
                currentSlider = EventSystem.current.currentSelectedGameObject.GetComponent<UI.Slider>();
                currentScrollbar = null;
                if (currentSlider.direction == UI.Slider.Direction.LeftToRight || currentSlider.direction == UI.Slider.Direction.RightToLeft)
                {
                    movementThreshold.x = scrollAmount * 4f;
                    movementThreshold.y = EasyInputHelper.mInstance.requiredSwipeLength;
                }
                else
                {
                    movementThreshold.x = EasyInputHelper.mInstance.requiredSwipeLength;
                    movementThreshold.y = scrollAmount * 4f;
                }
            }
            else
            {
                currentScrollbar = null;
                currentSlider = null;
                movementThreshold.x = EasyInputHelper.mInstance.requiredSwipeLength;
                movementThreshold.y = EasyInputHelper.mInstance.requiredSwipeLength;
            }


            //touch (here we're simulating the nice Apple UI with swipe naviagtion)
            //we are essentially going to keep track of the touch and fire off a 1 value only when the swipe
            //threshold has been reached

            if (touchpad.currentTouchPosition != EasyInputConstants.NOT_TOUCHING)
            {
                movement = touchpad.currentTouchPosition - touchpad.originalTouchPosition;

                //horizontal
                if (movement.x > ((movementThreshold.x * horizontalStepsFired) + movementThreshold.x))
                {
                    movement.x = 1;
                    horizontalStepsFired++;

                    if (currentScrollbar != null || currentSlider != null)
                    {
                        movement.x = scrollSpeedMultiplier;
                        horizontalStepsFired += (scrollSpeedMultiplier - 1);
                    }

                }
                else if (movement.x < ((movementThreshold.x * horizontalStepsFired) - movementThreshold.x))
                {
                    movement.x = -1;
                    horizontalStepsFired--;

                    if (currentScrollbar != null || currentSlider != null)
                    {
                        movement.x = -scrollSpeedMultiplier;
                        horizontalStepsFired -= (scrollSpeedMultiplier - 1);
                    }
                }
                else
                {
                    movement.x = 0;
                }

                //vertical
                if (movement.y > ((movementThreshold.y * verticalStepsFired) + movementThreshold.y))
                {
                    movement.y = 1;
                    verticalStepsFired++;

                    if (currentScrollbar != null || currentSlider != null)
                    {
                        movement.y = scrollSpeedMultiplier;
                        verticalStepsFired += (scrollSpeedMultiplier - 1);
                    }
                }
                else if (movement.y < ((movementThreshold.y * verticalStepsFired) - movementThreshold.y))
                {
                    movement.y = -1;
                    verticalStepsFired--;

                    if (currentScrollbar != null || currentSlider != null)
                    {
                        movement.y = -scrollSpeedMultiplier;
                        verticalStepsFired -= (scrollSpeedMultiplier - 1);
                    }
                }
                else
                {
                    movement.y = 0;
                }


            }

            if (currentScrollbar != null)
            {
                if (currentScrollbar.direction == UI.Scrollbar.Direction.LeftToRight)
                {
                    currentScrollbar.value += scrollAmount * movement.x;
                    movement.x = 0;
                }
                else if (currentScrollbar.direction == UI.Scrollbar.Direction.RightToLeft)
                {
                    currentScrollbar.value += -scrollAmount * movement.x;
                    movement.x = 0;
                }
                else if (currentScrollbar.direction == UI.Scrollbar.Direction.TopToBottom)
                {
                    currentScrollbar.value += -scrollAmount * movement.y;
                    movement.y = 0;
                }
                else if (currentScrollbar.direction == UI.Scrollbar.Direction.BottomToTop)
                {
                    currentScrollbar.value += scrollAmount * movement.y;
                    movement.y = 0;
                }
            }
            if (currentSlider != null)
            {
                if (currentSlider.direction == UI.Slider.Direction.LeftToRight)
                {
                    currentSlider.value += scrollAmount * movement.x;
                    movement.x = 0;
                }
                else if (currentSlider.direction == UI.Slider.Direction.RightToLeft)
                {
                    currentSlider.value += -scrollAmount * movement.x;
                    movement.x = 0;
                }
                else if (currentSlider.direction == UI.Slider.Direction.TopToBottom)
                {
                    currentSlider.value += -scrollAmount * movement.y;
                    movement.y = 0;
                }
                else if (currentSlider.direction == UI.Slider.Direction.BottomToTop)
                {
                    currentSlider.value += scrollAmount * movement.y;
                    movement.y = 0;
                }
            }

            var axisEventData = GetAxisEventData(movement.x, movement.y, 0.6f);
            if (!Mathf.Approximately(axisEventData.moveVector.x, 0f)
                || !Mathf.Approximately(axisEventData.moveVector.y, 0f))
            {
                ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
            }
            return axisEventData.used;
        }

        /// <summary>
        /// Process move navigation (dpad or joystick)
        /// </summary>
        private bool SendMoveEventToSelectedObject()
        {
            if ((DateTime.UtcNow - lastEvent).TotalSeconds < repeatEventRate)
                return false;


            Vector2 movement = Vector2.zero;

            //left stick
            if (movement == Vector2.zero)
            {
                    movement = leftStick;
            }

            //dpad (only if not populated yet)
            if (movement == Vector2.zero)
            {
                    movement = dpad;
            }

            


            var axisEventData = GetAxisEventData(movement.x, movement.y, 0.6f);
            if (!Mathf.Approximately(axisEventData.moveVector.x, 0f)
                || !Mathf.Approximately(axisEventData.moveVector.y, 0f))
            {
                lastEvent = DateTime.UtcNow;
                ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
            }
            return axisEventData.used;
        }

        private bool SendUpdateEventToSelectedObject()
        {
            if (eventSystem.currentSelectedGameObject == null)
                return false;

            var data = GetBaseEventData();
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.updateSelectedHandler);
            return data.used;
        }

        void localClickStart (ButtonClick button)
        {
            if (buttonMode == EasyInputConstants.INPUT_MODULE_BUTTON_MODE.FireAtRepeatRate || buttonMode == EasyInputConstants.INPUT_MODULE_BUTTON_MODE.FireOnceAtButtonDown)
            {
                if (button.button == EasyInputConstants.CONTROLLER_BUTTON.AButton || button.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTouchClick)
                {
                    submit = true;
                }
                if (button.button == EasyInputConstants.CONTROLLER_BUTTON.BButton || button.button == EasyInputConstants.CONTROLLER_BUTTON.Back)
                {
                    cancel = true;
                }
            }
            
            //check if we should only do for one frame
            if (buttonMode == EasyInputConstants.INPUT_MODULE_BUTTON_MODE.FireOnceAtButtonDown)
            {
                if (button.button == EasyInputConstants.CONTROLLER_BUTTON.AButton || button.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTouchClick)
                {
                    submitForOneFrame = true;
                }
                if (button.button == EasyInputConstants.CONTROLLER_BUTTON.BButton || button.button == EasyInputConstants.CONTROLLER_BUTTON.Back)
                {
                    cancelForOneFrame = true;
                }
            }

            if (button.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTrigger || button.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRHMDPadTap)
                triggerDownThisFrame = true;

        }

        void localClickEnd(ButtonClick button)
        {
            if (buttonMode == EasyInputConstants.INPUT_MODULE_BUTTON_MODE.FireAtRepeatRate || buttonMode == EasyInputConstants.INPUT_MODULE_BUTTON_MODE.FireOnceAtButtonDown)
            {
                if (button.button == EasyInputConstants.CONTROLLER_BUTTON.AButton || button.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTouchClick)
                {
                    submit = false;
                }
                if (button.button == EasyInputConstants.CONTROLLER_BUTTON.BButton || button.button == EasyInputConstants.CONTROLLER_BUTTON.Back)
                {
                    cancel = false;
                }
            }
            else
            {
                if (button.button == EasyInputConstants.CONTROLLER_BUTTON.AButton || button.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTouchClick)
                {
                    submit = true;
                    submitForOneFrame = true;
                }
                if (button.button == EasyInputConstants.CONTROLLER_BUTTON.BButton || button.button == EasyInputConstants.CONTROLLER_BUTTON.Back)
                {
                    cancel = true;
                    cancelForOneFrame = true;
                }
            }

            if (button.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTrigger || button.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRHMDPadTap)
                triggerUpThisFrame = true;
        }

        void localLeftStick(ControllerAxis axis)
        {
            leftStick = axis.axisValue;
        }

        void localDpad(ControllerAxis axis)
        {
            dpad = axis.axisValue;

        }

        void localTouch(InputTouch touch)
        {
            touchpad = touch;
        }

        void localTouchEnd(InputTouch touch)
        {
            touchpad.currentTouchPosition = EasyInputConstants.NOT_TOUCHING;
            horizontalStepsFired = 0;
            verticalStepsFired = 0;

        }

        public void setUIRay(Vector3 origin, Quaternion orientation, float distance)
        {
            rayOrigin = origin;
            rayOrientation = orientation;
            uiMaxDistance = distance;
                    
        }

        public Vector3 getuiHitPosition()
        {
            return uiHitPosition;
        }

        
         /// <summary>
        /// Process the current mouse press.
        /// </summary>
        protected void ProcessPointer()
        {
            
            var pointerEvent = eventData;
            var currentOverGo = pointerEvent.pointerCurrentRaycast.gameObject;

            // PointerDown notification
            if (triggerDownThisFrame)
            {
                triggerDownThisFrame = false;
                pointerEvent.eligibleForClick = true;
                pointerEvent.delta = Vector2.zero;
                pointerEvent.dragging = false;
                pointerEvent.useDragThreshold = true;
                pointerEvent.pressPosition = pointerEvent.position;
                pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;

                DeselectIfSelectionChanged(currentOverGo, pointerEvent);

                // search for the control that will receive the press
                // if we can't find a press handler set the press
                // handler to be what would receive a click.
                var newPressed = ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, ExecuteEvents.pointerDownHandler);

                // didnt find a press handler... search for a click handler
                if (newPressed == null)
                    newPressed = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

                // Debug.Log("Pressed: " + newPressed);

                float time = Time.unscaledTime;

                if (newPressed == pointerEvent.lastPress)
                {
                    var diffTime = time - pointerEvent.clickTime;
                    if (diffTime < 0.3f)
                        ++pointerEvent.clickCount;
                    else
                        pointerEvent.clickCount = 1;

                    pointerEvent.clickTime = time;
                }
                else
                {
                    pointerEvent.clickCount = 1;
                }

                pointerEvent.pointerPress = newPressed;
                pointerEvent.rawPointerPress = currentOverGo;

                pointerEvent.clickTime = time;

                // Save the drag handler as well
                pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(currentOverGo);

                if (pointerEvent.pointerDrag != null)
                {
                    ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.initializePotentialDrag);
                }
            }

            // PointerUp notification
            if (triggerUpThisFrame)
            {
                triggerUpThisFrame = false;
                // Debug.Log("Executing pressup on: " + pointer.pointerPress);
                ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);

                // Debug.Log("KeyCode: " + pointer.eventData.keyCode);

                // see if we mouse up on the same element that we clicked on...
                var pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

                // PointerClick and Drop events
                if (pointerEvent.pointerPress == pointerUpHandler && pointerEvent.eligibleForClick)
                {
                    ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);
                }
                else if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
                {
                    ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, ExecuteEvents.dropHandler);
                }

                pointerEvent.eligibleForClick = false;
                pointerEvent.pointerPress = null;
                pointerEvent.rawPointerPress = null;

                if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
                {
                    ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
                }

                pointerEvent.dragging = false;
                pointerEvent.pointerDrag = null;

                // redo pointer enter / exit to refresh state
                // so that if we moused over somethign that ignored it before
                // due to having pressed on something else
                // it now gets it.
                if (currentOverGo != pointerEvent.pointerEnter)
                {
                    HandlePointerExitAndEnter(pointerEvent, null);
                    HandlePointerExitAndEnter(pointerEvent, currentOverGo);
                }
            }
        }


       
    }
}