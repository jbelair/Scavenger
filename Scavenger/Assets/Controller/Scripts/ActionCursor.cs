using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class ActionCursor : ActionInputBase
{
    public string axisX;
    public bool invertX = false;
    public string axisY;
    public bool invertY = false;

    public float sensitivity = 1f;
    public float strength = 2f;

    public Vector2 lastInput = Vector2.zero;

    public Vector2 GetInput()
    {
        Vector2 input = lastInput;// Vector2.zero;

        float iX = (invertX) ? -1 : 1;
        float iY = (invertY) ? -1 : 1;

        if (axisX != "" && axisY != "")
        {
            input += Vector2.right * Input.GetAxis(axisX) * iX;
            input += Vector2.up * Input.GetAxis(axisY) * iY;

            if (input.magnitude < sensitivity)
                input = Vector2.zero;
            else
            {
                input.Normalize();
                input.Set((Mathf.Abs(input.x) - sensitivity) / (1f - sensitivity) * ((input.x < 0) ? -1f : 1f), (Mathf.Abs(input.y) - sensitivity) / (1f - sensitivity) * ((input.y < 0) ? -1f : 1f));
            }

            input *= strength;
        }
        else
        {
            input = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;
            //if (Physics.Raycast(ray, out hit))
            //    input = hit.point;//Input.mousePosition;
        }

        lastInput = input;

        return input;
    }
}
