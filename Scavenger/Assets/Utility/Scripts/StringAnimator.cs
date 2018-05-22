using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class StringSequence
{
    public string name = "";
    public StringSequencer sequence;
}

public class StringAnimator : MonoBehaviour
{
    [Header("State")]
    public string active = "";
    public string lastActive = "";
    public string value = "";
    public UnityEvent listeners;

    [Header("Animations")]
    public StringSequence[] animations;

    // Use this for initialization
    void Start()
    {
        value = animations[NameToIndex(active)].sequence.Start();

        value = StringHelper.CommandParse(value);

        listeners.Invoke();

        lastActive = active;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastActive != active)
            value = animations[NameToIndex(active)].sequence.Start();
        //else
        //    value = strings[NameToIndex(active)].sequence.Update();

        //value = StringHelper.CommandParse(value);

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

    public void SetStringOnText(Text text)
    {
        text.text = value;
    }
}
