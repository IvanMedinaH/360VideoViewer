using UnityEngine;
using System.Collections;
using EasyInputVR.Core;
using UnityEngine.Events;

namespace EasyInputVR.StandardControllers
{

    [AddComponentMenu("EasyInputGearVR/Standard Controllers/StandardPointerReceiver")]
    public class StandardPointerReceiver : StandardBaseReceiver
    {

        //events
        [System.Serializable]
        public class HitHandler : UnityEvent<Vector3> { }
        [SerializeField]
        public HitHandler onHover;
        [SerializeField]
        public HitHandler onHoverEnter;
        [SerializeField]
        public HitHandler onHoverExit;


        bool fireEvent;
        bool clicking;


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        }

        public override void Hover(Vector3 hitPosition, Transform pointerTransform)
        {
            onHover.Invoke(hitPosition);
        }

        public override void HoverEnter(Vector3 hitPosition, Transform pointerTransform)
        {
            onHoverEnter.Invoke(hitPosition);
        }

        public override void HoverExit(Vector3 hitPosition, Transform pointerTransform)
        {
            onHoverExit.Invoke(hitPosition);
        }
        
    }

}
