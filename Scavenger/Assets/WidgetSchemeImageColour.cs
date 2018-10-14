using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class WidgetSchemeImageColour : MonoBehaviour
{
    public Image image;
    public string scheme = "Default";

    // Use this for initialization
    void Start()
    {
        if (!image)
            image = GetComponent<Image>();

        image.color = WidgetScheme.Scheme(scheme).colour;
    }
}
