using UnityEngine;
using System.Collections;

namespace EasyInputVR.Core
{

    public static class EasyInputUtilities
    {
        public static float determineDistance(Vector2 original, Vector2 current)
        {
            float distance = Mathf.Sqrt (Mathf.Abs ((Mathf.Pow ((original.x-current.x),2)) + (Mathf.Pow ((original.y - current.y), 2) ) ) );
            return distance;            
        }

        public static EasyInputConstants.SWIPE_TYPE determineSwipeType(InputTouch myTouch)
        {
            EasyInputConstants.SWIPE_TYPE swipeType = EasyInputConstants.SWIPE_TYPE.None;

            Vector2 difference = myTouch.currentTouchPosition - myTouch.originalTouchPosition;

            if (Mathf.Abs(difference.x) > Mathf.Abs(difference.y))
            {
                //horizontal
                swipeType = (difference.x > 0f) ? EasyInputConstants.SWIPE_TYPE.Right : EasyInputConstants.SWIPE_TYPE.Left;
            }
            else
            {
                //vertical
                swipeType = (difference.y > 0f) ? EasyInputConstants.SWIPE_TYPE.Up : EasyInputConstants.SWIPE_TYPE.Down;
            }

            return swipeType;
        }

        //This method is used to calculate a vector3 from a 2 axis control
        public static Vector3 getControllerVector3(float horizontalMovement, float verticalMovement, EasyInputConstants.AXIS horizontalAxis, EasyInputConstants.AXIS verticalAxis)
        {
            Vector3 controllerVector;
            controllerVector.x = 0f; controllerVector.y = 0f; controllerVector.z = 0f;

            //if either axis are mapped to x or negative x
            if (horizontalAxis == EasyInputConstants.AXIS.XAxis)
                controllerVector.x += horizontalMovement;
            if (verticalAxis == EasyInputConstants.AXIS.XAxis)
                controllerVector.x += verticalMovement;
            if (horizontalAxis == EasyInputConstants.AXIS.NegativeXAxis)
                controllerVector.x -= horizontalMovement;
            if (verticalAxis == EasyInputConstants.AXIS.NegativeXAxis)
                controllerVector.x -= verticalMovement;

            //if either axis are mapped to y
            if (horizontalAxis == EasyInputConstants.AXIS.YAxis)
                controllerVector.y += horizontalMovement;
            if (verticalAxis == EasyInputConstants.AXIS.YAxis)
                controllerVector.y += verticalMovement;
            if (horizontalAxis == EasyInputConstants.AXIS.NegativeYAxis)
                controllerVector.y -= horizontalMovement;
            if (verticalAxis == EasyInputConstants.AXIS.NegativeYAxis)
                controllerVector.y -= verticalMovement;

            //if either axis are mapped to z
            if (horizontalAxis == EasyInputConstants.AXIS.ZAxis)
                controllerVector.z += horizontalMovement;
            if (verticalAxis == EasyInputConstants.AXIS.ZAxis)
                controllerVector.z += verticalMovement;
            if (horizontalAxis == EasyInputConstants.AXIS.NegativeZAxis)
                controllerVector.z -= horizontalMovement;
            if (verticalAxis == EasyInputConstants.AXIS.NegativeZAxis)
                controllerVector.z -= verticalMovement;

            //if either axis is none it isn't taken into account so no action needed


            //return the vector
            return controllerVector;
        }


        //Implementation limitation: the vectors cannot be parallel to n
        public static float relativeAngleInAxis(Vector3 firstVector, Vector3 secondVector, Vector3 axisVector)
        {
            Vector3 tmp, a, b;

            axisVector.Normalize();

            tmp = Vector3.Cross(axisVector, firstVector);
            a = Vector3.Cross(tmp, axisVector);

            tmp = Vector3.Cross(axisVector, secondVector);
            b = Vector3.Cross(tmp, axisVector);

            tmp = Vector3.Cross(a, b);
            float angleSignTmp = Vector3.Angle(tmp, axisVector) - 90;
            float angleSign = Mathf.Sign(angleSignTmp);
            return (Vector3.Angle(a, b) * angleSign);
        }

        public static Vector3 dampenSensorNoise(Vector3 currentRawValue, Vector3 previouslySmoothedValue, float threshold, int framesWithinThresholdX, int framesWithinThresholdY, int framesWithinThresholdZ)
        {
            //Implementation Notes: For Acceleration and Gravity there are 2 special cases. If the value is within the
            //threshold of 0 or 1 (common accelerometer values) we are going to force the value to exactly 0 or 1
            //to help minimize velcoity and position drift. If it's not within the threshold of either 0 or 1
            //we are going to compare the current raw value and the previously smoothed value and produce a new
            //smoothed value

            Vector3 newSmoothedValue = currentRawValue; //just default the value to raw
            bool nearSpecialValueX = false , nearSpecialValueY = false, nearSpecialValueZ = false;

            //zero checks
            if (currentRawValue.x >= -threshold && currentRawValue.x <= threshold)
            {
                newSmoothedValue.x = 0f;
                nearSpecialValueX = true;
            }
            if (currentRawValue.y >= -threshold && currentRawValue.y <= threshold)
            {
                newSmoothedValue.y = 0f;
                nearSpecialValueY = true;
            }
            if (currentRawValue.z >= -threshold && currentRawValue.z <= threshold)
            {
                newSmoothedValue.z = 0f;
                nearSpecialValueZ = true;
            }

            //one checks
            if (currentRawValue.x >= (-threshold + 1) && currentRawValue.x <= (threshold + 1))
            {
                newSmoothedValue.x = 1f;
                nearSpecialValueX = true;
            }
            if (currentRawValue.y >= (-threshold + 1) && currentRawValue.y <= (threshold + 1))
            {
                newSmoothedValue.y = 1f;
                nearSpecialValueY = true;
            }
            if (currentRawValue.z >= (-threshold + 1) && currentRawValue.z <= (threshold + 1))
            {
                newSmoothedValue.z = 1f;
                nearSpecialValueZ = true;
            }

            //minus one checks
            if (currentRawValue.x >= (-threshold - 1) && currentRawValue.x <= (threshold - 1))
            {
                newSmoothedValue.x = -1f;
                nearSpecialValueX = true;
            }
            if (currentRawValue.y >= (-threshold - 1) && currentRawValue.y <= (threshold - 1))
            {
                newSmoothedValue.y = -1f;
                nearSpecialValueY = true;
            }
            if (currentRawValue.z >= (-threshold - 1) && currentRawValue.z <= (threshold - 1))
            {
                newSmoothedValue.z = -1f;
                nearSpecialValueZ = true;
            }


            //not a special case smooth out the value based on previous smoothed value only if we have one
            if (previouslySmoothedValue == EasyInputConstants.NOT_VALID)
            {
                //we have no previous value just return the current one
                //this has already been assigned no action necessary
            }
            else
            {
                if (!nearSpecialValueX)
                {
                    //smooth X if the values are within the threshold of one another
                    if (currentRawValue.x >= (-threshold + previouslySmoothedValue.x) && currentRawValue.x <= (threshold + previouslySmoothedValue.x))
                    {
                        //we're within the threshold so smooth
                        newSmoothedValue.x = (newSmoothedValue.x + ((float)framesWithinThresholdX * previouslySmoothedValue.x)) / (float)(framesWithinThresholdX + 1);
                    }
                }
                if (!nearSpecialValueY)
                {
                    //smooth Y if the values are within the threshold of one another
                    if (currentRawValue.y >= (-threshold + previouslySmoothedValue.y) && currentRawValue.y <= (threshold + previouslySmoothedValue.y))
                    {
                        //we're within the threshold so smooth
                        newSmoothedValue.y = (newSmoothedValue.y + ((float)framesWithinThresholdY * previouslySmoothedValue.y)) / (float)(framesWithinThresholdY + 1);
                    }
                }
                if (!nearSpecialValueZ)
                {
                    //smooth Z if the values are within the threshold of one another
                    if (currentRawValue.z >= (-threshold + previouslySmoothedValue.z) && currentRawValue.z <= (threshold + previouslySmoothedValue.z))
                    {
                        //we're within the threshold so smooth
                        newSmoothedValue.z = (newSmoothedValue.z + ((float)framesWithinThresholdZ * previouslySmoothedValue.z)) / (float)(framesWithinThresholdZ + 1);
                    }
                }

            }


            return newSmoothedValue;
        }

        public static void notifyEvents(RaycastHit rayHit, Vector3 lastRayHit, GameObject lastHitGameObject, bool hover, bool hoverEnter, bool hoverExit, Transform pointer)
        {

            if (hoverExit)
            {
                var exitReceivers = lastHitGameObject.GetComponents<StandardControllers.StandardBaseReceiver>();

                foreach (var receiver in exitReceivers)
                {
                    receiver.HoverExit(lastRayHit, pointer);
                }
            }

            if (hover || hoverEnter)
            {
                var receivers = rayHit.transform.gameObject.GetComponents<StandardControllers.StandardBaseReceiver>();
                foreach (var receiver in receivers)
                {
                    if (hover)
                        receiver.Hover(rayHit.point, pointer);
                    if (hoverEnter)
                        receiver.HoverEnter(rayHit.point, pointer);
                }
            }
        }

    }
}
