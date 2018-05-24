using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ControllerAction
{
    public static Dictionary<string, ControllerAction> Actions;

    public string action;
    public UnityEvent<ControllerUEI, Vector2, float> start;
    public UnityEvent<ControllerUEI> end;
}