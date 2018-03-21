using System;
using UnityEngine;

[Serializable]
public class ActionKey : ActionInputBase
{
    public KeyCode positive;
    public KeyCode negative;

    public float lastInput = 0;

    public float GetInput()
    {
        float input = 0;

        if(Input.anyKey)
        {
            if (Input.GetKey(positive))
                input += 1;
            if (Input.GetKey(negative))
                input -= 1;
        }

        lastInput = input;

        return input;
    }
}
