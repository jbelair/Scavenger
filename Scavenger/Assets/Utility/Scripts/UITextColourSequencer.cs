using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UITextColourSequencer : MonoBehaviour
{
    public Text text;

    public bool tint;
    public Color baseColour;

    public ColourSequencer sequence;

    // Use this for initialization
    void Start()
    {
        if (tint)
        {
            baseColour = text.color;
            text.color = baseColour * sequence.Start();
        }
        else
            text.color = sequence.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (tint)
            text.color = baseColour * sequence.Update();
        else
            text.color = sequence.Update();
    }
}
