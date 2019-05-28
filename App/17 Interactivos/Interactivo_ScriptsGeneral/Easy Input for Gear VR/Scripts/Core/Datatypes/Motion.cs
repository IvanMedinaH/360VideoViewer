using UnityEngine;
using System.Collections;

namespace EasyInputVR.Core
{
    public class Motion
    {
        //Position tracking over time also becomes inaccurate but that is due to limitations with a gyro/accel/mag only setup.
        //in addition to infering postion over time (without knowing starting state) so its best to use with a starting
        //state of the remote being still when reseting    

        //real data
        public float lastResetTimestamp;
        public Quaternion currentOrientation;
        public Vector3 currentOrientationEuler;
        public Vector3 currentAngVel;
        public Vector3 currentPos;
        public Vector3 currentAcc;

        //derived data
        public Vector3 currentVel;
        public Vector3 lastFrameVel;
        public Vector3 posSinceLastReset;
        public Vector3 totalVelSinceLastReset;
        public Vector3 totalAngularVelSinceLastReset;
    }
}
