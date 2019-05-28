using UnityEngine;
using System.Collections;
using EasyInputVR.Core;
using System;

namespace EasyInputVR.StandardControllers
{

    [AddComponentMenu("EasyInputGearVR/Standard Controllers/StandardComboPointer")]
    public class StandardComboPointer : MonoBehaviour
    {
        public GameObject laserPointer;
        public GameObject hmd;
        public float laserHeightOffset = 0f;
        public Material laserMaterial;
        public Color laserStartColor;
        public Color laserEndColor;
        public float laserDistance = .5f;
        public GameObject reticle;
        public float reticleDistance = 5f;
        public Color reticleColor;
        public bool UIRaycast;
        public UnityEngine.EventSystems.EasyInputModule InputModule;
        public bool colliderRaycast;
        public LayerMask layersToCheck;

        bool laserInteraction = true;
        EasyInputConstants.TOUCH_DEVICE currentDevice;
        LineRenderer line;
        RaycastHit rayHit;
        Vector3 end;
        Vector3 offsetPosition;
        Vector3 initialPosition = EasyInputConstants.NOT_VALID;
        Vector3 initialReticleSize;
        Vector3 uiHitPosition;
        GameObject lastHitGameObject;
        Vector3 lastRayHit;

        public void OnEnable()
        {
            EasyInputHelper.On_Motion += localMotion;

        }

        public void OnDestroy()
        {
            EasyInputHelper.On_Motion -= localMotion;
        }

        void Start()
        {

            if (reticle != null)
            {
                initialReticleSize = reticle.transform.localScale;
            }

            line = laserPointer.AddComponent<LineRenderer>();
            line.material = laserMaterial;
#if UNITY_5_3 || UNITY_5_4
            line.SetWidth(0.01f, 0.01f);
            line.SetVertexCount(2);
            line.SetColors(laserStartColor, laserEndColor);
#endif
#if UNITY_5_5
            line.startColor = laserStartColor;
            line.endColor = laserEndColor;
            line.startWidth = .01f;
            line.endWidth = .01f;
            line.numPositions = 2;
#endif
#if !(UNITY_5_3 || UNITY_5_4 || UNITY_5_5)
            line.startColor = laserStartColor;
            line.endColor = laserEndColor;
            line.startWidth = .01f;
            line.endWidth = .01f;
            line.positionCount = 2;
#endif

            if (laserPointer.transform.parent == null)
                initialPosition = laserPointer.transform.position;

        }

        void Update()
        {
            currentDevice = EasyInputHelper.touchDevice;
        }

        void localMotion(EasyInputVR.Core.Motion motion)
        {
            if (laserPointer == null || hmd == null || !this.gameObject.activeInHierarchy)
                return;

            if (reticle != null)
            {
                reticle.SetActive(true);
            }

            if (currentDevice == EasyInputConstants.TOUCH_DEVICE.HMD || motion.currentPos == Vector3.zero)
            {
                //hmd mode
                end = EasyInputConstants.NOT_VALID;
                line.enabled = false;
                laserPointer.SetActive(false);

                if (colliderRaycast && Physics.Raycast(hmd.transform.position, hmd.transform.forward, out rayHit, reticleDistance, layersToCheck))
                {
                    end = rayHit.point;
                    if (rayHit.transform != null && rayHit.transform.gameObject != null)
                    {
                        if (lastHitGameObject == null)
                        {
                            //we weren't hitting anything before and now we are
                            EasyInputUtilities.notifyEvents(rayHit, lastRayHit, lastHitGameObject, true, true, false, hmd.transform);
                        }
                        else if (lastHitGameObject == rayHit.transform.gameObject)
                        {

                            //we are hitting the same object as last frame
                            EasyInputUtilities.notifyEvents(rayHit, lastRayHit, lastHitGameObject, true, false, false, hmd.transform);
                        }
                        else if (lastHitGameObject != rayHit.transform.gameObject)
                        {
                            //we are hitting a different object than last frame
                            EasyInputUtilities.notifyEvents(rayHit, lastRayHit, lastHitGameObject, true, true, true, hmd.transform);
                        }

                        lastHitGameObject = rayHit.transform.gameObject;
                        lastRayHit = rayHit.point;
                    }
                }

                if (end != EasyInputConstants.NOT_VALID)
                {
                    if (reticle != null)
                    {
                        reticle.transform.position = end;
                        reticle.transform.localScale = initialReticleSize * .6f * (Mathf.Sqrt((end - hmd.transform.position).magnitude / reticleDistance));
                    }
                }
                else
                {
                    //didn't hit anything
                    if (colliderRaycast)
                    {
                        //raycast enabled but didn't hit anything
                        if (lastHitGameObject != null)
                        {
                            EasyInputUtilities.notifyEvents(rayHit, lastRayHit, lastHitGameObject, false, false, true, hmd.transform);
                            lastHitGameObject = null;
                            lastRayHit = EasyInputConstants.NOT_VALID;
                        }
                    }

                    if (reticle != null)
                    {
                        reticle.transform.position = hmd.transform.position + hmd.transform.forward * reticleDistance;
                        reticle.transform.localScale = initialReticleSize;
                    }
                }

                if (reticle != null)
                    reticle.GetComponent<MeshRenderer>().material.color = reticleColor;

                //UI based interactions
                if (UIRaycast && InputModule != null)
                {
                    InputModule.setUIRay(hmd.transform.position, hmd.transform.rotation, reticleDistance);
                    uiHitPosition = InputModule.getuiHitPosition();
                    if (uiHitPosition != EasyInputConstants.NOT_VALID && (end == EasyInputConstants.NOT_VALID || (end - hmd.transform.position).magnitude > (uiHitPosition - hmd.transform.position).magnitude))
                    {
                        if ((uiHitPosition - hmd.transform.position).magnitude < reticleDistance)
                        {
                            reticle.transform.position = uiHitPosition;
                            reticle.transform.localScale = initialReticleSize * .6f * (Mathf.Sqrt((uiHitPosition - hmd.transform.position).magnitude / reticleDistance));
                        }
                    }
                }
            }
            else
            {
                //laser pointer mode

                offsetPosition = motion.currentPos;
                offsetPosition.y += laserHeightOffset;

                if (laserPointer.transform.parent == null)
                    laserPointer.transform.localPosition = initialPosition + offsetPosition;
                else
                    laserPointer.transform.localPosition = offsetPosition;


                laserPointer.transform.localRotation = motion.currentOrientation;

                if (motion.currentPos != Vector3.zero && laserInteraction == true)
                {
                    line.enabled = true;
                    laserPointer.SetActive(true);
                    if (reticle != null)
                        reticle.SetActive(true);
                }
                else
                {
                    //not valid so disable and don't bother with the raycast so return
                    line.enabled = false;
                    laserPointer.SetActive(false);
                    if (laserInteraction == false)
                    {
                        //if we've turned off the laser we do want the hand to show just not the laser
                        laserPointer.SetActive(true);
                    }
                    if (reticle != null)
                        reticle.SetActive(false);
                    return;
                }

                end = EasyInputConstants.NOT_VALID;

                //origin
                line.SetPosition(0, laserPointer.transform.position);


                if (colliderRaycast && Physics.Raycast(laserPointer.transform.position, laserPointer.transform.forward, out rayHit, reticleDistance, layersToCheck))
                {
                    end = rayHit.point;
                    if (rayHit.transform != null && rayHit.transform.gameObject != null)
                    {
                        if (lastHitGameObject == null)
                        {
                            //we weren't hitting anything before and now we are
                            EasyInputUtilities.notifyEvents(rayHit, lastRayHit, lastHitGameObject, true, true, false, laserPointer.transform);
                        }
                        else if (lastHitGameObject == rayHit.transform.gameObject)
                        {

                            //we are hitting the same object as last frame
                            EasyInputUtilities.notifyEvents(rayHit, lastRayHit, lastHitGameObject, true, false, false, laserPointer.transform);
                        }
                        else if (lastHitGameObject != rayHit.transform.gameObject)
                        {
                            //we are hitting a different object than last frame
                            EasyInputUtilities.notifyEvents(rayHit, lastRayHit, lastHitGameObject, true, true, true, laserPointer.transform);
                        }

                        lastHitGameObject = rayHit.transform.gameObject;
                        lastRayHit = rayHit.point;
                    }
                }

                //endpoint
                //line.SetPosition(1, end);

                if (end != EasyInputConstants.NOT_VALID)
                {
                    if (reticle != null)
                    {
                        reticle.transform.position = end;
                        reticle.transform.localScale = initialReticleSize * .6f * (Mathf.Sqrt((end - laserPointer.transform.position).magnitude / reticleDistance));
                    }

                    if ((end - laserPointer.transform.position).magnitude < laserDistance)
                    {
                        line.SetPosition(1, end);
                    }
                    else
                    {
                        line.SetPosition(1, laserPointer.transform.position + laserPointer.transform.forward * laserDistance);
                    }

                }
                else
                {
                    //didn't hit anything
                    if (colliderRaycast)
                    {
                        //raycast enabled but didn't hit anything
                        if (lastHitGameObject != null)
                        {
                            EasyInputUtilities.notifyEvents(rayHit, lastRayHit, lastHitGameObject, false, false, true, laserPointer.transform);
                            lastHitGameObject = null;
                            lastRayHit = EasyInputConstants.NOT_VALID;
                        }
                    }

                    if (reticle != null)
                    {
                        reticle.transform.position = laserPointer.transform.position + laserPointer.transform.forward * reticleDistance;
                        reticle.transform.localScale = initialReticleSize;
                    }


                    line.SetPosition(1, laserPointer.transform.position + laserPointer.transform.forward * laserDistance);

                }

                if (reticle != null)
                    reticle.GetComponent<MeshRenderer>().material.color = reticleColor;

                //UI based interactions
                if (UIRaycast && InputModule != null && (motion.currentPos != Vector3.zero))
                {
                    InputModule.setUIRay(laserPointer.transform.position, laserPointer.transform.rotation, reticleDistance);
                    uiHitPosition = InputModule.getuiHitPosition();
                    if (uiHitPosition != EasyInputConstants.NOT_VALID && (end == EasyInputConstants.NOT_VALID || (end - laserPointer.transform.position).magnitude > (uiHitPosition - laserPointer.transform.position).magnitude))
                    {
                        if ((uiHitPosition - laserPointer.transform.position).magnitude < reticleDistance)
                        {
                            reticle.transform.position = uiHitPosition;
                            reticle.transform.localScale = initialReticleSize * .6f * (Mathf.Sqrt((uiHitPosition - laserPointer.transform.position).magnitude / reticleDistance));
                        }
                    }
                }
            }

        }

        public void setInitialScale(Vector3 scale)
        {
            initialReticleSize = scale;
        }

        public void startLaser()
        {
            laserInteraction = true;
        }

        public void stopLaser()
        {
            laserInteraction = false;
        }



    }

}

