using UnityEngine;
using System.Collections;
using EasyInputVR.Core;
using System;

namespace EasyInputVR.StandardControllers
{

    [AddComponentMenu("EasyInputGearVR/Standard Controllers/StandardGazePointer")]
    public class StandardGazePointer : MonoBehaviour
    {
        public GameObject reticle;
        public float reticleDistance = 5f;
        public Color reticleColor;
        public bool UIRaycast;
        public UnityEngine.EventSystems.EasyInputModule InputModule;
        public bool colliderRaycast;
        public LayerMask layersToCheck;

        GameObject hmd;
        RaycastHit rayHit;
        Vector3 end;
        StandardBaseReceiver receiver;
        Vector3 initialReticleSize;
        Vector3 uiHitPosition;
        GameObject lastHitGameObject;
        Vector3 lastRayHit;

        void Start()
        {
            hmd = this.gameObject;

            if (reticle != null)
            {
                initialReticleSize = reticle.transform.localScale;
            }
        }

        void Update()
        {
            if (reticle != null)
            {
                reticle.SetActive(true);
            }

            end = EasyInputConstants.NOT_VALID;

            //Physics based interactions
            if (colliderRaycast && Physics.Raycast(this.transform.position, this.transform.forward, out rayHit, reticleDistance, layersToCheck))
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

        public void setInitialScale(Vector3 scale)
        {
            initialReticleSize = scale;
        }

      


    }

}

