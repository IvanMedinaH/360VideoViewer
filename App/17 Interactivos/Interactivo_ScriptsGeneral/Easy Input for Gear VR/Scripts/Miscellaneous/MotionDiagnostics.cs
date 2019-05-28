using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using EasyInputVR.Core;

namespace EasyInputVR.Misc
{

    [AddComponentMenu("EasyInputGearVR/Miscellaneous/MotionDiagnostics")]
    public class MotionDiagnostics : MonoBehaviour
    {
        public Text curOrientationXValue;
        public Text curOrientationYValue;
        public Text curOrientationZValue;
        public Text curAngVelXValue;
        public Text curAngVelYValue;
        public Text curAngVelZValue;
        public Text curPosXValue;
        public Text curPosYValue;
        public Text curPosZValue;
        public Text curAccXValue;
        public Text curAccYValue;
        public Text curAccZValue;
        public Text curDerVelXValue;
        public Text curDerVelYValue;
        public Text curDerVelZValue;
        public Text curDerPosXValue;
        public Text curDerPosYValue;
        public Text curDerPosZValue;
        public Text curTotVelSinceResetXValue;
        public Text curTotVelSinceResetYValue;
        public Text curTotVelSinceResetZValue;
        public Text curTotAngVelSinceResetXValue;
        public Text curTotAngVelSinceResetYValue;
        public Text curTotAngVelSinceResetZValue;



        void OnEnable()
        {
            EasyInputHelper.On_Motion += localMotion;
            EasyInputHelper.On_ClickStart += localClickStart;

        }

        void OnDestroy()
        {
            EasyInputHelper.On_Motion -= localMotion;
            EasyInputHelper.On_ClickStart -= localClickStart;
        }

        void Update()
        {
            
    }



        void localMotion (EasyInputVR.Core.Motion motion)
        {
            curOrientationXValue.text = motion.currentOrientationEuler.x.ToString();
            curOrientationYValue.text = motion.currentOrientationEuler.y.ToString();
            curOrientationZValue.text = motion.currentOrientationEuler.z.ToString();
            curAngVelXValue.text = motion.currentAngVel.x.ToString();
            curAngVelYValue.text = motion.currentAngVel.y.ToString();
            curAngVelZValue.text = motion.currentAngVel.z.ToString();
            curPosXValue.text = motion.currentPos.x.ToString();
            curPosYValue.text = motion.currentPos.y.ToString();
            curPosZValue.text = motion.currentPos.z.ToString();
            curAccXValue.text = motion.currentAcc.x.ToString();
            curAccYValue.text = motion.currentAcc.y.ToString();
            curAccZValue.text = motion.currentAcc.z.ToString();
            curDerVelXValue.text = motion.currentVel.x.ToString();
            curDerVelYValue.text = motion.currentVel.y.ToString();
            curDerVelZValue.text = motion.currentVel.z.ToString();
            curDerPosXValue.text = motion.posSinceLastReset.x.ToString();
            curDerPosYValue.text = motion.posSinceLastReset.y.ToString();
            curDerPosZValue.text = motion.posSinceLastReset.z.ToString();
            curTotVelSinceResetXValue.text = motion.totalVelSinceLastReset.x.ToString();
            curTotVelSinceResetYValue.text = motion.totalVelSinceLastReset.y.ToString();
            curTotVelSinceResetZValue.text = motion.totalVelSinceLastReset.z.ToString();
            curTotAngVelSinceResetXValue.text = motion.totalAngularVelSinceLastReset.x.ToString();
            curTotAngVelSinceResetYValue.text = motion.totalAngularVelSinceLastReset.y.ToString();
            curTotAngVelSinceResetZValue.text = motion.totalAngularVelSinceLastReset.z.ToString();
        }

        void localClickStart(ButtonClick button)
        {
            if (button.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTrigger)
                EasyInputHelper.resetMotion();

        }

    }
}
