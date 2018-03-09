using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InputAxis
{
    public enum Axis { LeftStickX, LeftStickY, RightStickX, RightStickY, LeftTrigger, RightTrigger };

    public string name;
    public Axis axis;

    public string Name
    {
        get
        {
            switch(axis)
            {
                case Axis.LeftStickX:
                    return "Left Horizontal";
                case Axis.LeftStickY:
                    return "Left Vertical";
                case Axis.RightStickX:
                    return "Right Horizontal";
                case Axis.RightStickY:
                    return "Right Vertical";
                case Axis.LeftTrigger:
                    return "Left Trigger";
                case Axis.RightTrigger:
                    return "Right Trigger";
            }

            return "";
        }
    }
}

[Serializable]
public class InputButton
{
    public enum Button { DPadUp, DPadDown, DPadLeft, DPadRight, A, X, Y, B, LeftBumper, RightBumper, Back, Start, LeftStick, RightStick };

    public string name;
    public Button button;

    public string Name
    {
        get
        {
            switch(button)
            {
                case Button.DPadUp:
                    return "D-Pad Up";
                case Button.DPadDown:
                    return "D-Pad Down";
                case Button.DPadLeft:
                    return "D-Pad Left";
                case Button.DPadRight:
                    return "D-Pad Right";
                case Button.A:
                    return "A";
                case Button.X:
                    return "X";
                case Button.Y:
                    return "Y";
                case Button.B:
                    return "B";
                case Button.LeftBumper:
                    return "Left Bumper";
                case Button.RightBumper:
                    return "Right Bumper";
                case Button.Back:
                    return "Back";
                case Button.Start:
                    return "Start";
                case Button.LeftStick:
                    return "Left Stick";
                case Button.RightStick:
                    return "Right Stick";
            }
            return "";
        }
    }
}

public class ControllerUEI : MonoBehaviour
{
    public enum OS { Windows, MacOSX, Linux };
    public enum Player { Any, Player1, Player2, Player3, Player4, Player5, Player6, Player7, Player8 };

    public static string OSString(OS os)
    {
        switch(os)
        {
            case OS.Windows:
                return "W";
            case OS.MacOSX:
                return "M";
            case OS.Linux:
                return "L";
        }
        return "";
    }
    public static string PlayerString(Player player)
    {
        switch(player)
        {
            case Player.Any:
                return "";
            case Player.Player1:
                return "P1";
            case Player.Player2:
                return "P2";
            case Player.Player3:
                return "P3";
            case Player.Player4:
                return "P4";
            case Player.Player5:
                return "P5";
            case Player.Player6:
                return "P6";
            case Player.Player7:
                return "P7";
            case Player.Player8:
                return "P8";
        }
        return "";
    }

    public OS os;
    public Player player;

    public List<InputAxis> axis = new List<InputAxis>();
    public List<InputButton> buttons = new List<InputButton>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
