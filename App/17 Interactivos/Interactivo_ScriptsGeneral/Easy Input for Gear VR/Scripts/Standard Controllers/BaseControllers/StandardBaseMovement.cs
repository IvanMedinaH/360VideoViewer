using UnityEngine;
using System.Collections;
using EasyInputVR.Core;
using UnityEngine.Events;

namespace EasyInputVR.StandardControllers
{
    public abstract class StandardBaseMovement : MonoBehaviour
    {
        public abstract void blockMovement();
        public abstract void unblockMovement();
    }

}