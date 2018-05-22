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
            if (action.start == null)
                action.start = new UnityEvent();

            if (action.end == null)
                action.end = new UnityEvent();
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
    }
}
