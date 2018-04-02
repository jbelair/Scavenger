using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageColourSequencer : MonoBehaviour
{
    public Image image;
    public ColourSequencer colours;

    void Awake()
    {
        if (!image)
            image = GetComponent<Image>();
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