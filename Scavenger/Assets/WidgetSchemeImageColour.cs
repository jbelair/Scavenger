using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class WidgetSchemeImageColour : MonoBehaviour
{
    public Image image;
    public string scheme = "Default";

    [Serializable]
    public class Key
    {
        public string scheme;
        public Color colour;
    }
    public List<Key> keys = new List<Key>();

    public float transitionTime = 0.5f;
    public float currentTime = 0.0f;

    // Use this for initialization
    void Start()
    {
        if (!image)
            image = GetComponent<Image>();

        Set();
    }

    void Update()
    {
        if (keys.Count > 1)
        {
            if (currentTime >= transitionTime)
            {
                currentTime -= transitionTime;
                keys.RemoveAt(0);
                image.color = keys[0].colour;
            }

            currentTime += Time.deltaTime;
            if (keys.Count > 1)
            {
                image.color = Color.Lerp(keys[0].colour, keys[1].colour, currentTime / transitionTime);
            }
            else
            {
                image.color = keys[0].colour;
            }

        }
        else
        {
            image.color = keys[0].colour;
        }
    }

    public void Set()
    {
        Set(scheme);
    }

    public void Set(string newScheme)
    {
        //if (keys.Count > 0)
        //    if (keys[keys.Count - 1].scheme == scheme)
        //        keys.RemoveAt(keys.Count - 1);
        while (keys.Count > 2)
        {
            keys.RemoveAt(2);
        }
        keys.Add(new Key() { scheme = scheme, colour = WidgetScheme.Scheme(newScheme).colour });
    }
}
