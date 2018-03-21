using System;
using UnityEngine;

[Serializable]
public class ActionAxis : ActionInputBase
{
    public string axis;
    public bool invert = false;

    [Range(0, 0.2f)]
    public float sensitivity = 0.15f;

    public float lastInput = 0;

    public float GetInput()
    {
        float input = Input.GetAxisRaw(axis) * ((invert) ? -1 : 1);

        if (input < sensitivity)
            input = 0;
        else
            input = (input - sensitivity) / (1 - sensitivity);

        lastInput = input;

        return input;
    }
}
