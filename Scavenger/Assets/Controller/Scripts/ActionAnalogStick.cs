using System;
using UnityEngine;

[Serializable]
public class ActionAnalogStick : ActionInputBase
{
    public KeyCode up;
    public KeyCode left;
    public KeyCode down;
    public KeyCode right;

    public string axisX;
    public bool invertX = false;
    public string axisY;
    public bool invertY = false;

    [Range(0, 0.5f)]
    public float sensitivity = 0.15f;

    public Vector2 lastInput;

    public Vector2 GetInput()
    {
        Vector2 input = Vector2.zero;

        float iX = (invertX) ? -1 : 1;
        float iY = (invertY) ? -1 : 1;

        if (Input.anyKey && (up != KeyCode.None || left != KeyCode.None || down != KeyCode.None || right != KeyCode.None))
        {
            if (Input.GetKey(up))
                input += Vector2.up * iY;
            if (Input.GetKey(left))
                input += Vector2.right * iX;
            if (Input.GetKey(down))
                input += Vector2.down * iY;
            if (Input.GetKey(right))
                input += Vector2.left * iX;
        }
        else if (axisX == axisY && axisX == "Mouse Position")
        {
            input = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0);
        }
        else if (axisX != "" && axisY != "")
        {
            input += Vector2.right * Input.GetAxis(axisX) * iX;
            input += Vector2.up * Input.GetAxis(axisY) * iY;
        }

        if (input.magnitude < sensitivity)
            input = Vector2.zero;
        else
        {
            if (input.magnitude > 1)
                input.Normalize();

            input = input.normalized * (input.magnitude - sensitivity) / (1f - sensitivity);
        }

        lastInput = input;

        return input;
    }
}
