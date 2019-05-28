using UnityEngine;
using System.Collections;
using EasyInputVR.Core;
using UnityEngine.Events;

namespace EasyInputVR.StandardControllers
{

    [AddComponentMenu("EasyInputGearVR/Standard Controllers/StandardPointerClickReceiver")]
    public class StandardPointerClickReceiver : StandardBaseReceiver
    {
        public EasyInputConstants.BUTTON_CONDITION buttonCondition = EasyInputConstants.BUTTON_CONDITION.GearVRTriggerPressed;

        //events
        [System.Serializable]
        public class HitHandler : UnityEvent<Vector3,ButtonClick> { }
        [SerializeField]
        public HitHandler onHoverClick;
        [SerializeField]
        public HitHandler onHoverClickStart;
        [SerializeField]
        public HitHandler onHoverClickEnd;
        [SerializeField]
        public HitHandler onHoverQuickClickEnd;
        [SerializeField]
        public HitHandler onHoverDoubleClickEnd;
        [SerializeField]
        public HitHandler onHoverLongClick;
        [SerializeField]
        public HitHandler onHoverLongClickStart;
        [SerializeField]
        public HitHandler onHoverLongClickEnd;

        bool isHovering;
        bool clickStart;
        bool clicking;
        bool clickEnd;
        bool longClicking;
        bool longClickStart;
        bool longClickEnd;
        bool quickClickEnd;
        bool doubleClickEnd;
        ButtonClick currentButton;
        Vector3 currentPosition;



        void OnEnable()
        {
            EasyInputHelper.On_ClickEnd += clickEndMethod;
            EasyInputHelper.On_Click += clickMethod;
            EasyInputHelper.On_ClickStart += clickStartMethod;
            EasyInputHelper.On_LongClick += longClickMethod;
            EasyInputHelper.On_LongClickEnd += longClickEndMethod;
            EasyInputHelper.On_LongClickStart += longClickStartMethod;
            EasyInputHelper.On_DoubleClickEnd += doubleClickEndMethod;
            EasyInputHelper.On_QuickClickEnd += quickClickEndMethod;

        }

        void OnDestroy()
        {
            EasyInputHelper.On_ClickEnd -= clickEndMethod;
            EasyInputHelper.On_Click -= clickMethod;
            EasyInputHelper.On_ClickStart -= clickStartMethod;
            EasyInputHelper.On_LongClick -= longClickMethod;
            EasyInputHelper.On_LongClickEnd -= longClickEndMethod;
            EasyInputHelper.On_LongClickStart -= longClickStartMethod;
            EasyInputHelper.On_DoubleClickEnd -= doubleClickEndMethod;
            EasyInputHelper.On_QuickClickEnd -= quickClickEndMethod;

        }


        // Update is called once per frame
        void Update()
        {
            if (isHovering)
            {
                if (clicking)
                {
                    onHoverClick.Invoke(currentPosition, currentButton);
                    if (clickStart)
                        onHoverClickStart.Invoke(currentPosition, currentButton);
                    if (longClickStart)
                        onHoverLongClickStart.Invoke(currentPosition, currentButton);
                    if (longClicking)
                        onHoverLongClick.Invoke(currentPosition, currentButton);
                }
                else
                {
                    if (clickEnd)
                        onHoverClickEnd.Invoke(currentPosition, currentButton);
                    if (longClickEnd)
                        onHoverLongClickEnd.Invoke(currentPosition, currentButton);
                    if (quickClickEnd)
                        onHoverQuickClickEnd.Invoke(currentPosition, currentButton);
                    if (doubleClickEnd)
                        onHoverDoubleClickEnd.Invoke(currentPosition, currentButton);
                }
            }


            //reset variables
            currentButton = null;
            currentPosition = EasyInputConstants.NOT_VALID;
            isHovering = false;
            clickStart = false;
            clicking = false;
            clickEnd = false;
            longClicking = false;
            longClickStart = false;
            longClickEnd = false;
            quickClickEnd = false;
            doubleClickEnd = false;


        }

        public override void Hover(Vector3 hitPosition, Transform pointerTransform)
        {
            isHovering = true;
            currentPosition = hitPosition;
        }

        public override void HoverEnter(Vector3 hitPosition, Transform pointerTransform)
        {
        }

        public override void HoverExit(Vector3 hitPosition, Transform pointerTransform)
        {
            isHovering = false;
            currentPosition = hitPosition;
        }


        void clickEndMethod(ButtonClick button)
        {
            if ((int)button.button == (int)buttonCondition)
            {
                clickEnd = true;
                clicking = false;
                currentButton = button;
            }

        }

        void clickMethod(ButtonClick button)
        {
            if ((int)button.button == (int)buttonCondition)
            {
                clicking = true;
                currentButton = button;
            }
        }

        void clickStartMethod(ButtonClick button)
        {
            if ((int)button.button == (int)buttonCondition)
            {
                clickStart = true;
                clicking = true;
                currentButton = button;
            }
        }

        void longClickEndMethod(ButtonClick button)
        {
            if ((int)button.button == (int)buttonCondition)
            {
                longClickEnd = true;
                longClicking = false;
                currentButton = button;
            }

        }

        void longClickMethod(ButtonClick button)
        {
            if ((int)button.button == (int)buttonCondition)
            {
                longClicking = true;
                currentButton = button;
            }
        }

        void longClickStartMethod(ButtonClick button)
        {
            if ((int)button.button == (int)buttonCondition)
            {
                longClickStart = true;
                longClicking = true;
                currentButton = button;
            }
        }

        void quickClickEndMethod(ButtonClick button)
        {
            if ((int)button.button == (int)buttonCondition)
            {
                quickClickEnd = true;
                clicking = false;
                currentButton = button;
            }

        }

        void doubleClickEndMethod(ButtonClick button)
        {
            if ((int)button.button == (int)buttonCondition)
            {
                doubleClickEnd = true;
                clicking = false;
                currentButton = button;
            }

        }


    }

}
