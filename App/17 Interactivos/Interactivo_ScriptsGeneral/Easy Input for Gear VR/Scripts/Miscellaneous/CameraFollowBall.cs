using UnityEngine;
using System.Collections;
namespace EasyInputVR.Misc
{

    [AddComponentMenu("EasyInputGearVR/Miscellaneous/CameraFollowBall")]
    public class CameraFollowBall : MonoBehaviour
    {

        public GameObject objectToFollow;

        Transform objectTransform;
        Vector3 newPosition;

        // Use this for initialization
        void Start()
        {

            objectTransform = objectToFollow.transform;
        }

        // Update is called once per frame
        void Update()
        {
            newPosition = objectTransform.position;
            newPosition.y = 2.0f;
            newPosition.z -= 5f;

            this.transform.position = newPosition;
        }
    }
}
