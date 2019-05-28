using UnityEngine;
using System.Collections;
using EasyInputVR.Core;
using System;

namespace EasyInputVR.StandardControllers
{

    [AddComponentMenu("EasyInputGearVR/Standard Controllers/StandardCurvedLaserPointer")]
    public class StandardCurvedLaserPointer : StandardBaseLaser
    {
        public float heightOffset = 0f;
        public Material laserMaterial;
        public Color laserStartColor;
        public Color laserEndColor;
        public int segmentsCount = 20;
        public float segmentLength = 1f;
        public float segmentCurveDegrees = 5f;
        public GameObject reticle;
        public Color reticleColor;
        public bool colliderRaycast;
        public LayerMask layersToCheck;
        public bool UIRaycast;
        public UnityEngine.EventSystems.EasyInputModule InputModule;


        GameObject laserPointer;
        LineRenderer line;
        RaycastHit rayHit;
        GameObject previous;
        Vector3 end;
        Vector3 offsetPosition;
        Vector3 initialPosition = EasyInputConstants.NOT_VALID;
        Vector3 initialReticleSize;
        float reticleDistance;
        GameObject lastHitGameObject;
        Vector3 lastRayHit;
        Vector3 uiHitPosition;
        bool showReticle;
        bool showLaser = true;


        void OnEnable()
        {
            EasyInputHelper.On_Motion += localMotion;

        }

        void OnDestroy()
        {
            EasyInputHelper.On_Motion -= localMotion;
        }

        void Start()
        {
            previous = new GameObject();

            laserPointer = this.gameObject;

            if (reticle != null)
            {
                initialReticleSize = reticle.transform.localScale;
                reticle.GetComponent<MeshRenderer>().material.color = reticleColor;
                showReticle = true;
            }
            else
            {
                showReticle = false;
            }

            previous.transform.position = laserPointer.transform.position;
            previous.transform.forward = laserPointer.transform.forward;
            for (int i = 1; i < segmentsCount; i++)
            {
                previous.transform.position = previous.transform.position + previous.transform.forward * segmentLength;
                previous.transform.rotation = Quaternion.AngleAxis(segmentCurveDegrees, previous.transform.right) * previous.transform.rotation;
            }

            reticleDistance = (previous.transform.position - laserPointer.transform.position).magnitude;


            line = laserPointer.AddComponent<LineRenderer>();
            line.material = laserMaterial;
#if UNITY_5_3 || UNITY_5_4
            line.SetWidth(0.01f, 0.01f);
            line.SetVertexCount(segmentsCount);
            line.SetColors(laserStartColor, laserEndColor);
#endif
#if UNITY_5_5
            line.startColor = laserStartColor;
            line.endColor = laserEndColor;
            line.startWidth = .01f;
            line.endWidth = .01f;
            line.numPositions = segmentsCount;
#endif
#if !(UNITY_5_3 || UNITY_5_4 || UNITY_5_5)
            line.startColor = laserStartColor;
            line.endColor = laserEndColor;
            line.startWidth = .01f;
            line.endWidth = .01f;
            line.positionCount = segmentsCount;
#endif

            if (laserPointer.transform.parent == null)
                initialPosition = laserPointer.transform.position;

        }

        void localMotion(EasyInputVR.Core.Motion motion)
        {
            //if null or not active don't do anything
            if (laserPointer == null || !this.gameObject.activeInHierarchy)
                return;


            //update the position and rotation of the laser pointer (not the laser itself)
            offsetPosition = motion.currentPos;
            offsetPosition.y += heightOffset;

            if (laserPointer.transform.parent == null)
                laserPointer.transform.localPosition = initialPosition + offsetPosition;
            else
                laserPointer.transform.localPosition = offsetPosition;

            laserPointer.transform.localRotation = motion.currentOrientation;



            //check if we should show the laser and if not return
            if (motion.currentPos != Vector3.zero && showLaser)
            {
                line.enabled = true;
            }
            else
            {
                line.enabled = false;
                return;
            }


            //calculations for the laser
            end = EasyInputConstants.NOT_VALID;

            //set the number of positions like we aren't going to hit anything
#if UNITY_5_3 || UNITY_5_4
            line.SetVertexCount(segmentsCount);
#endif
#if UNITY_5_5
            line.numPositions = segmentsCount;
#endif
#if !(UNITY_5_3 || UNITY_5_4 || UNITY_5_5)
            line.positionCount = segmentsCount;
#endif

            //origin
            line.SetPosition(0, laserPointer.transform.position);
            previous.transform.position = laserPointer.transform.position;
            previous.transform.forward = laserPointer.transform.forward;

            for (int i=1;i< segmentsCount;i++)
            {
                //first set the position like it didn't hit anything
                line.SetPosition(i, (previous.transform.position + previous.transform.forward * segmentLength));

                //now do the raycast
                if (colliderRaycast && Physics.Raycast(previous.transform.position, previous.transform.forward, out rayHit, segmentLength, layersToCheck))
                {
                    end = rayHit.point;
                    line.SetPosition(i,end);

                    //we hit something so adjust the number of positions
#if UNITY_5_3 || UNITY_5_4
                    line.SetVertexCount(i+1);
#endif
#if UNITY_5_5
                    line.numPositions = i+1;
#endif
#if !(UNITY_5_3 || UNITY_5_4 || UNITY_5_5)
                    line.positionCount = i+1;
#endif

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

                    //break out of the loop since we hit something
                    break;
                }

                previous.transform.position = previous.transform.position + previous.transform.forward * segmentLength;
                previous.transform.rotation = Quaternion.AngleAxis(segmentCurveDegrees, previous.transform.right) * previous.transform.rotation;


            }

            //hit something
            if (end != EasyInputConstants.NOT_VALID)
            {
                if (reticle != null && showReticle)
                {
                    reticle.SetActive(true);
                    reticle.transform.position = end;
                    reticle.transform.localScale = initialReticleSize * .6f * (Mathf.Sqrt((end - laserPointer.transform.position).magnitude / reticleDistance));
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
                    reticle.SetActive(false);
                    reticle.transform.position = previous.transform.position;
                    reticle.transform.localScale = initialReticleSize;
                }

            }

            //UI based interactions
            if (UIRaycast && InputModule != null && (motion.currentPos != Vector3.zero))
            {
                InputModule.setUIRay(laserPointer.transform.position, laserPointer.transform.rotation, reticleDistance);
                uiHitPosition = InputModule.getuiHitPosition();
                if (uiHitPosition != EasyInputConstants.NOT_VALID && (end == EasyInputConstants.NOT_VALID || (end - laserPointer.transform.position).magnitude > (uiHitPosition - laserPointer.transform.position).magnitude))
                {
                    reticle.SetActive(false);
                    if ((uiHitPosition - laserPointer.transform.position).magnitude < reticleDistance)
                    {
                        reticle.transform.position = uiHitPosition;
                        reticle.transform.localScale = initialReticleSize * .6f * (Mathf.Sqrt((uiHitPosition - laserPointer.transform.position).magnitude / reticleDistance));
                    }
                }
            }

        }

        public void setInitialScale(Vector3 scale)
        {
            initialReticleSize = scale;
        }

        public override void TurnOffLaserAndReticle()
        {
            showLaser = false;
            showReticle = false;
            if (reticle != null && this.gameObject.activeInHierarchy)
                reticle.SetActive(false);
        }

        public override void TurnOnLaserAndReticle()
        {
            showLaser = true;
            showReticle = true;
            if (reticle != null && this.gameObject.activeInHierarchy)
                reticle.SetActive(true);
        }



    }

}

