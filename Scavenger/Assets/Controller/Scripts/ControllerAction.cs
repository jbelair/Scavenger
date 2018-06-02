using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ControllerAction
{
    public static Dictionary<string, ControllerAction> Actions = new Dictionary<string, ControllerAction>();

    public string action;
    public ControllerUEI controller;
    public UnityEvent start;
    public UnityEvent end;
    //public UnityEvent<ControllerUEI, Vector2, float> start;
    //public UnityEvent<ControllerUEI> end;
}