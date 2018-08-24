using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageColourSequencer : MonoBehaviour
{
    public Image image;

    public bool tint = false;
    public Color baseColour;

    public ColourSequencer colours;

    // Use this for initialization
    void Start()
    {
        if (image)
            baseColour = image.color;

        if (tint)
            image.color = baseColour * colours.Start();
        else
            image.color = colours.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (tint)
            image.color = baseColour * colours.Update();
        else
            image.color = colours.Update();
    }
}