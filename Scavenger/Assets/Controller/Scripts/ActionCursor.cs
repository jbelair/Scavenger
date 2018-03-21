using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ActionCursor : ActionInputBase
{
    public string axisX;
    public bool invertX = false;
    public string axisY;
    public bool invertY = false;

    public float sensitivity = 1f;

    public Vector2 lastInput = Vector2.zero;

    public Vector2 GetInput()
    {
        Vector2 input = Vector2.zero;

        float iX = (invertX) ? -1 : 1;
        float iY = (invertY) ? -1 : 1;

        if (axisX != "" && axisY != "")
        {
            input += Vector2.right * Input.GetAxisRaw(axisX) * iX;
            input += Vector2.up * Input.GetAxisRaw(axisY) * iY;
        }

        return input;
    }
}
