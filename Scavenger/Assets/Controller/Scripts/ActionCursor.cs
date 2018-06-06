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

    public float sensitivity = 0.15f;
    //public float strength = 2f;

    public Vector3 lastInput = Vector3.zero;

    public Vector3 GetInput()
    {
        Vector3 input = lastInput;

        float iX = (invertX) ? -1 : 1;
        float iY = (invertY) ? -1 : 1;

        if (axisX != "" && axisY != "")
        {
            input += Vector3.right * Input.GetAxis(axisX) * iX;
            input += Vector3.up * Input.GetAxis(axisY) * iY;

            if (input.magnitude < sensitivity)
                input = Vector2.zero;
            else
            {
                input.Normalize();
                input.Set((Mathf.Abs(input.x) - sensitivity) / (1f - sensitivity) * ((input.x < 0) ? -1f : 1f), (Mathf.Abs(input.y) - sensitivity) / (1f - sensitivity) * ((input.y < 0) ? -1f : 1f), 0);
            }

            //input *= strength;
        }
        else
        {
            //input = Input.mousePosition;// - new Vector3(Screen.width/2, Screen.height/2);
            //input.Normalize();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float p = -(Vector3.Dot(ray.origin, Vector3.forward) / Vector3.Dot(ray.direction, Vector3.forward));

            input = ray.origin + ray.direction * p;
        }

        lastInput = input;

        return input;
    }
}
