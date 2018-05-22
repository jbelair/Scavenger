using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class ColourSequence
{
    public string name;
    public ColourSequencer sequence;
}

public class ColourAnimator : MonoBehaviour
{
    [Header("State")]
    public string active = "";
    public string lastActive = "";
    public Color value = Color.white;
    public UnityEvent listeners;

    [Header("Animations")]
    public ColourSequence[] animations;

    // Use this for initialization
    void Start()
    {
        value = animations[NameToIndex(active)].sequence.Start();

        listeners.Invoke();

        lastActive = active;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastActive != active)
            value = animations[NameToIndex(active)].sequence.Start();

        listeners.Invoke();
    }

    int NameToIndex(string name)
    {
        for (int i = 0; i < animations.Length; i++)
        {
            if (animations[i].name == name)
                return i;
        }

        return 0;
    }

    public void SetColourOnImage(Image image)
    {
        image.color = value;
    }

    public void SetColourOnRawImage(RawImage image)
    {
        image.color = value;
    }

    public void SetColourOnText(Text text)
    {
        text.color = value;
    }
}
