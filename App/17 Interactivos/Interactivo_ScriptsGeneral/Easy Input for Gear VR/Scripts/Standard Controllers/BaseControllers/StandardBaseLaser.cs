using UnityEngine;
using System.Collections;
using EasyInputVR.Core;
using UnityEngine.Events;

namespace EasyInputVR.StandardControllers
{
    public abstract class StandardBaseLaser : MonoBehaviour
    {
        public abstract void TurnOffLaserAndReticle();
        public abstract void TurnOnLaserAndReticle();
    }

}