using UnityEngine;

namespace EasyInputVR.Misc
{

    [AddComponentMenu("EasyInputGearVR/Miscellaneous/AnchorTextAbove")]
    public class AnchorTextAbove : MonoBehaviour
    {

        public string Text;

        TextMesh textTM;
        Camera mainCamera;

        // Use this for initialization
        void Start()
        {
            mainCamera = Camera.main;
            textTM = this.GetComponent<TextMesh>();
            textTM.text = Text;
        }

        // Update is called once per frame
        void Update()
        {
            textTM.transform.forward = mainCamera.transform.forward;
        }

    }
}