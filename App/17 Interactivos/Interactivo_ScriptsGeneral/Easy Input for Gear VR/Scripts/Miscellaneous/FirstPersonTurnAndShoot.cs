using UnityEngine;
using System.Collections;
using EasyInputVR.Core;

namespace EasyInputVR.Misc
{

    [AddComponentMenu("EasyInputGearVR/Miscellaneous/FirstPersonTurnAndShoot")]
    public class FirstPersonTurnAndShoot : MonoBehaviour
    {
        public GameObject prefabBall;
        public float buttonSensitivity = 3f;
        float accumulatedRotation = 0f;
        Vector3 rotation;
        public GameObject spawn;

        Transform spawnTransform;

        void OnEnable()
        {
            EasyInputHelper.On_LongClick += localOnLongClick;
            EasyInputHelper.On_QuickClickEnd += localOnQuickEnd;
            EasyInputHelper.On_DoubleClickEnd += localOnDoubleEnd;
        }

        void OnDestroy()
        {
            EasyInputHelper.On_LongClick -= localOnLongClick;
            EasyInputHelper.On_QuickClickEnd -= localOnQuickEnd;
            EasyInputHelper.On_DoubleClickEnd -= localOnDoubleEnd;

        }

        void Start()
        {
            spawnTransform = spawn.transform;
        }

        public void Update()
        {
            rotation = this.transform.rotation.eulerAngles;
            rotation.y = accumulatedRotation;
            this.transform.rotation = Quaternion.Euler(rotation);
        }

        void localOnQuickEnd(ButtonClick click)
        {
            if (click.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTrigger)
            {
                GameObject newObject = (GameObject)Instantiate(prefabBall, spawnTransform.transform.position, spawnTransform.transform.rotation);
                Rigidbody newRigidbody = newObject.GetComponent<Rigidbody>();
                newRigidbody.AddForce((spawnTransform.forward) * 1000f);
            }

        }

        void localOnDoubleEnd(ButtonClick click)
        {
            if (click.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTrigger)
            {
                GameObject newObject = (GameObject)Instantiate(prefabBall, spawnTransform.transform.position, spawnTransform.transform.rotation);
                Rigidbody newRigidbody = newObject.GetComponent<Rigidbody>();
                newRigidbody.AddForce((spawnTransform.forward) * 1000f);
            }

        }

        void localOnLongClick(ButtonClick click)
        {
            if (click.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTrigger)
            {
                accumulatedRotation -= buttonSensitivity * Time.deltaTime * 100f;
            }
            else if (click.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTouchClick)
            {
                accumulatedRotation += buttonSensitivity * Time.deltaTime * 100f;
            }

        }
    }
}