using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSystem : MonoBehaviour
{
    public Material WaterMaterial;
    public float speed = 0.1F;

    // Use this for initialization
    void Update()
    {
        //Debug.Log(WaterMaterial.GetVector("_GAmplitude"));
        if (Input.GetKeyDown("q"))
        {
            Vector4 currentAmplitude = WaterMaterial.GetVector("_GAmplitude");
            Debug.Log(currentAmplitude.z);
           // WaterMaterial.SetVector("_GAmplitude", Vector4.Lerp(currentAmplitude, (2f, 2f, 2f ,2f), Time.deltaTime));
        }

    }

}
