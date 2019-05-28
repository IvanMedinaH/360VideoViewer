using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    public void EnterToInteractiveBattery()
    {
        SceneManager.LoadScene("Interactivo LTH");
    }


    public void EnterToInteractiveOtima()
    {
        SceneManager.LoadScene("ProductoBateria");
    }


    public void EnterToInteractiveLineaProductos()
    {
        SceneManager.LoadScene("BateriasAudio");
    }

}
