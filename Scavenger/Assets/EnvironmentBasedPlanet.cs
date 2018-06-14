using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentBasedPlanet : MonoBehaviour
{
    public Statistics environment;
    public Material planet;
    public Material atmosphere;

    // Use this for initialization
    void Start()
    {
        transform.localScale = Vector3.one * (Mathf.Log(environment[name + " Radius"] * 100f, 10) + 1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
