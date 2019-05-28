using UnityEngine;
using System.Collections;
using EasyInputVR.Core;
using UnityEngine.Events;

namespace EasyInputVR.StandardControllers
{
    public abstract class StandardBaseReceiver : MonoBehaviour
    {
        public abstract void Hover(Vector3 hitPosition, Transform pointerTransform);
        public abstract void HoverEnter(Vector3 hitPosition, Transform pointerTransform);
        public abstract void HoverExit(Vector3 hitPosition, Transform pointerTransform);
    }

}
