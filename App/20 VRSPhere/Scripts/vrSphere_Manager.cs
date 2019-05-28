using System;
using System.Collections.Generic;
using UnityEngine;

class vrSphere_Manager: MonoBehaviour
{
    public Sphere sphere;




    void Update()
    {
        sphere = GameObject.FindGameObjectWithTag("vrPlayer360").GetComponent<Sphere>();
        sphere.pivotPoint = GameObject.FindGameObjectWithTag("Player").transform.position;
    }


    public void grow_lerpTransformPosition()
    {
        sphere.transform.localScale = Vector3.Lerp(sphere.transform.localScale, sphere.Finalsize, sphere.lerpspeed * Time.deltaTime);

    }

    public void storeLastScale( Vector3 scale) {
        sphere.temp_LastScale = scale;

    }

    public void Shrink_lerpTransformPosition()
    {
        sphere.transform.localScale = Vector3.Lerp(sphere.transform.localScale, sphere.temp_LastScale, sphere.lerpspeed * Time.deltaTime * 2.0f);

    }

    public void centerSphereInPivot(Transform pivot)
    {
        sphere.SphereContainer.transform.position = pivot.position;
    }

    public void Hide_Vrsphere()
    {
        sphere.gameObject.SetActive(false);
    }

    public void show_Vrsphere()
    {
        sphere.gameObject.SetActive(true);
    }


}
