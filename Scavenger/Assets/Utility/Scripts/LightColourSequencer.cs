using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightColourSequencer : MonoBehaviour
{
    public Light light;
    public ColourSequencer colours;

    void Awake()
    {
        if (!light)
            light = GetComponent<Light>();
    }

    // Use this for initialization
    void Start()
    {
        light.color = colours.Start();
    }

    // Update is called once per frame
    void Update()
    {
        light.color = colours.Update();
    }
}