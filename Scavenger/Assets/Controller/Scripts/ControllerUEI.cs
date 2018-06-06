using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum InputMode { Gamepad, GamepadSouthpaw, GamepadAlt, GamepadAltSouthpaw, KeyboardAndMouse, KeyboardAndMouseAlt };

[RequireComponent(typeof(Statistics))]
public class ControllerUEI : MonoBehaviour
{
    public Statistics statistics;

    public InputMode mode = InputMode.Gamepad;
    public List<InputAction> actions = new List<InputAction>();

    void Awake()
    {
        foreach (InputAction action in actions)
        {
            //if (action.start == null)
            //    action.start = new UnityEvent<string>();

            //if (action.end == null)
            //    action.end = new UnityEvent<string>();
        }

        if (!statistics.Has("Mouse Position"))
            statistics["Mouse Position"] = new Statistic("Mouse Position", Statistic.ValueType.Vector2, (Vector2)Input.mousePosition);

        if (!statistics.Has("Mouse World Position"))
        {
            Vector3 mouse = Input.mousePosition;
            mouse.z = -Camera.main.transform.position.z;
            mouse = Camera.main.ScreenToWorldPoint(mouse);
            statistics["Mouse World Position"] = new Statistic("Mouse World Position", Statistic.ValueType.Vector3, mouse);
        }
    }

    // Use this for initialization
    void Start()
    {
        foreach (InputAction action in actions)
        {
            action.controller = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Since Unity Input has no callback support I need to poll the state of the input here
        // And determine which actions must be called, based on this.

        foreach (InputAction action in actions)
        {
            action.Update();
            // Check key bindings
        }

        statistics["Mouse Position"].Set((Vector2)Input.mousePosition);

        Vector3 mouse = Input.mousePosition;
        mouse.z = -Camera.main.transform.position.z;
        mouse = Camera.main.ScreenToWorldPoint(mouse);
        statistics["Mouse World Position"].Set(mouse);
    }
}
