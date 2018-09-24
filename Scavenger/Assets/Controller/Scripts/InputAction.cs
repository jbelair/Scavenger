using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class InputActionEvent : UnityEvent<ControllerUEI, string>
{

}

[Serializable]
public class InputAction
{
    public string name;
    public ControllerUEI controller;
    public bool enabled = true;
    public bool toggle = false;
    public bool isToggled = false;
    public bool ended = false;

    public ActionKey[] keys;
    public ActionAxis[] axis;
    public ActionAnalogStick[] sticks;
    public ActionCursor[] cursors;
    public string action;
    public InputActionEvent start;
    public InputActionEvent end;

    private readonly Statistic statistic;

    public void Update()
    {
        if (!enabled)
            return;

        // Check keybindings
        bool end = true;

        float iAxis = 0;
        Vector2 iStick = Vector2.zero;
        Vector3 iPosition = Vector3.zero;

        foreach (ActionKey key in keys)
        {
            if (key.Enabled(Application.platform, controller.mode))
            {
                iAxis += key.GetInput();
                if (iAxis != 0)
                {
                    Input(iPosition, iStick, iAxis);
                    ended = end = false;
                }
            }
        }

        foreach (ActionAxis axis in axis)
        {
            if (axis.Enabled(Application.platform, controller.mode))
            {
                iAxis += axis.GetInput();
                if (iAxis != 0)
                {
                    Input(iPosition, iStick, iAxis);
                    ended = end = false;
                }
            }
        }

        foreach (ActionAnalogStick stick in sticks)
        {
            if (stick.Enabled(Application.platform, controller.mode))
            {
                iStick += stick.GetInput();
                if (iStick.magnitude > 0)
                {
                    Input(iPosition, iStick, iAxis);
                    ended = end = false;
                }
            }
        }

        foreach (ActionCursor cursor in cursors)
        {
            if (cursor.Enabled(Application.platform, controller.mode))
            {
                iPosition += cursor.GetInput();
                if (iPosition.magnitude > 0)
                {
                    Input(iPosition, iStick, iAxis);
                    ended = end = false;
                }
            }
        }

        if (end && !ended)
        {
            EndInput();
            ended = true;
        }
    }

    public void Input(Vector3 inputVector3, Vector2 inputVector2, float inputFloat = 0)
    {
        // Perform only UnityEvents
        start.Invoke(controller, name);

        if (inputVector3 != Vector3.zero)
        {
            if (controller.statistics.Has(name))
                controller.statistics[name].Set(inputVector3);
            else
                controller.statistics[name] = new Statistic(name, Statistic.ValueType.Vector3, inputVector3);
        }
        else if (inputVector2 != Vector2.zero)
        {
            if (controller.statistics.Has(name))
                controller.statistics[name].Set(inputVector2);
            else
                controller.statistics[name] = new Statistic(name, Statistic.ValueType.Vector2, inputVector2);
        }
        else
        {
            if (controller.statistics.Has(name))
                controller.statistics[name].Set(inputFloat);
            else
                controller.statistics[name] = new Statistic(name, Statistic.ValueType.Float, inputFloat);
        }
    }

    public void EndInput()
    {
        // Perform only UnityEvents
        end.Invoke(controller, name);
        controller.statistics[name].Default();
    }
}
