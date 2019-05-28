/*using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using EasyInputVR.Core;
using System;
using System.Collections.Generic;

namespace EasyInputVR.StandardControllers
{

    [AddComponentMenu("EasyInputGearVR/Custom Controllers/CustomMotionController")]
    public class CustomMotionController : MonoBehaviour
    {

        //events
        [System.Serializable] public class AccelerometerHandler : UnityEvent<Vector3> { }
        [System.Serializable] public class GyroHandler : UnityEvent<Vector3,Vector3> { }
        [SerializeField]
        public AccelerometerHandler onAccelerometer;
        [SerializeField]
        public GyroHandler onGyro;



        void OnEnable()
        {
            EasyInputHelper.On_Accelerometer += localAcceleromter;
            EasyInputHelper.On_Gyro += localGyro;
        }

        void OnDestroy()
        {
            EasyInputHelper.On_Accelerometer -= localAcceleromter;
            EasyInputHelper.On_Gyro -= localGyro;
        }

        // Update is called once per frame
        void Update()
        {
        }

        void localAcceleromter(Vector3 accel)
        {
                onAccelerometer.Invoke(accel);
        }

        void localGyro(Vector3 gravity, Vector3 userAccel)
        {
                onGyro.Invoke(gravity,userAccel);
        }

        
    }

}*/

