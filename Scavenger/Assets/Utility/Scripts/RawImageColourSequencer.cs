using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class RawImageColourSequencer : MonoBehaviour
{
    public RawImage image;
    public ColourSequencer colours;

    void Awake()
    {
        image = GetComponent<RawImage>();
    }

    // Use this for initialization
    void Start()
    {
        image.color = colours.Start();
    }

    // Update is called once per frame
    void Update()
    {
        image.color = colours.Update();
    }
}
