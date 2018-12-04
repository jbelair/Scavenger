using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float scale = EnvironmentRules.RadiusOfJupiter;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = -Camera.main.transform.position / scale;
    }
}
