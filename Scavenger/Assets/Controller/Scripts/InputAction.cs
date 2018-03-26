using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class InputAction
{
    public enum Action { Custom, Select, Deselect, Skill0, Skill1, Skill2, Skill3, Equip, Equip0, Equip1, Equip2, Equip3, Unequip, Unequip0, Unequip1, Unequip2, Unequip3, MoveDirectional, MoveDirectionalX, MoveDirectionalY, MoveTargeted, AimDirectional, AimDirectionalX, AimDirectionalY, AimTargeted, ZoomIn, ZoomOut, Zoom, ToggleView, Guide, Menu, Statistics };

    public string name;
    public ControllerUEI controller;
    public bool enabled = true;
    public bool toggle = false;
    public bool isToggled = false;

    public ActionKey[] keys;
    public ActionAxis[] axis;
    public ActionAnalogStick[] sticks;
    public ActionCursor[] cursors;
    public Action action;
    public UnityEvent start;
    public UnityEvent end;

    public void Update()
    {
        if (!enabled)
            return;

        // Check keybindings
        bool end = true;

        float iAxis = 0;
        Vector2 iStick = Vector2.zero;

        foreach (ActionKey key in keys)
        {
            if (key.Enabled(Application.platform, controller.mode))
            {
                iAxis += key.GetInput();
                if (iAxis != 0)
                {
                    Input(iStick, iAxis);
                    end = false;
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
                    Input(iStick, iAxis);
                    end = false;
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
                    Input(iStick, iAxis);
                    end = false;
                }
            }
        }

        foreach (ActionCursor cursor in cursors)
        {
            if (cursor.Enabled(Application.platform, controller.mode))
            {
                iStick += cursor.GetInput();
                if (iStick.magnitude > 0)
                {
                    Input(iStick, iAxis);
                    end = false;
                }
            }
        }

        if (end)
            EndInput();
    }

    public void Input(Vector2 inputVector2, float inputFloat = 0)
    {
        Vector2 input = Vector2.zero;
        switch (action)
        {
            case Action.Custom:
                // Perform only UnityEvents
                start.Invoke();
                break;
            case Action.Select:
                // Perform select action
                // This needs to tie into 2 disparate systems, acting as their bridge between user interfacing and action
                // In UI we have the selected object
                // In Game we have the selected object
                // Here we would send a message to both selection systems that the player is actually trying to do something
                break;
            case Action.Deselect:
                // Similarly in deselect we need to tell the 2 systems that whatever is currently selected needs to be deselected
                // Or in the case of UI this may also mean back out of the current menu, and return to the previous menu (often one higher in the hierarchy of menues)
                break;
            case Action.Skill0:
                // This needs to interface with the player and attempt to activate skill 0
                break;
            case Action.Skill1:
                // This needs to interface with the player and attempt to activate skill 1
                break;
            case Action.Skill2:
                // This needs to interface with the player and attempt to activate skill 2
                break;
            case Action.Skill3:
                // This needs to interface with the player and attempt to activate skill 3
                break;
            case Action.Equip:
                // This needs to contextually determine what equip means.
                // This is the generic equip call that leaves determining where to equip a skill to later logic
                // As such it is at it's easiest, an opportunity to determine which if any skill slots are empty, in rising order, and equip the skill in the first discovered.
                // If the player is full this needs to allow them to chose which skill to replace.
                break;
            case Action.Equip0:
                // This specifically equips to skill 0, whatever is supplied in the context.
                break;
            case Action.Equip1:
                // This specifically equips to skill 1, whatever is supplied in the context.
                break;
            case Action.Equip2:
                // This specifically equips to skill 2, whatever is supplied in the context.
                break;
            case Action.Equip3:
                // This specifically equips to skill 3, whatever is supplied in the context.
                break;
            case Action.MoveDirectional:
                // This supplies movement input information to the player, in the form of direction to move in.
                // This requires an ActionAnalogStick be defined.
                controller.statistics["Movement Input"].Set(inputVector2);
                break;
            case Action.MoveDirectionalX:
                // This supplies movement input information to the player, in the form of direction to move in.
                input = controller.statistics["Movement Input"].Get<Vector2>();
                input.x = inputFloat;
                controller.statistics["Movement Input"].Set(input);
                break;
            case Action.MoveDirectionalY:
                // This supplies movement input information to the player, in the form of direction to move in.
                input = controller.statistics["Movement Input"].Get<Vector2>();
                input.y = inputFloat;
                controller.statistics["Movement Input"].Set(input);
                break;
            case Action.MoveTargeted:
                // This supplies movement input information to the player, in the form of location to reach.
                controller.statistics["Movement Input"].Set(inputVector2);
                break;
            case Action.AimDirectional:
                controller.statistics["Aim Input"].Set(inputVector2);
                break;
            case Action.AimDirectionalX:
                // This supplies aim input information to the player, in the form of direction to aim in.
                input = controller.statistics["Aim Input"].Get<Vector2>();
                input.x = inputFloat;
                controller.statistics["Aim Input"].Set(input);
                break;
            case Action.AimDirectionalY:
                // This supplies aim input information to the player, in the form of direction to aim in.
                input = controller.statistics["Aim Input"].Get<Vector2>();
                input.y = inputFloat;
                controller.statistics["Aim Input"].Set(input);
                break;
            case Action.AimTargeted:
                // This supplies aim input information to the player, in the form of location to aim at.
                controller.statistics["Aim Input"].Set(inputVector2);
                break;
            case Action.ZoomIn:
                // This zooms the camera in (transition from top down to 3rd person view, strategic to pilot.)
                break;
            case Action.ZoomOut:
                // This zooms the camera out (transition from 3rd person view to top down, pilot to strategic.)
                break;
            case Action.Zoom:
                // This zooms responding to an axis
                break;
            case Action.ToggleView:
                // This immediately toggles from strategic view to pilot view.
                break;
            case Action.Guide:
                // This enables any non basic guide view information (options for what that entails in game options.)
                break;
            case Action.Menu:
                // This opens the UI in game menu.
                break;
            case Action.Statistics:
                // This enables the detailed player statistics overlay.
                break;
        }
    }

    public void EndInput()
    {
        Vector2 input = Vector2.zero;
        switch (action)
        {
            case Action.Custom:
                end.Invoke();
                break;
            case Action.Select:
                break;
            case Action.Deselect:
                break;
            case Action.Skill0:
                break;
            case Action.Skill1:
                break;
            case Action.Skill2:
                break;
            case Action.Skill3:
                break;
            case Action.Equip:
                break;
            case Action.Equip0:
                break;
            case Action.Equip1:
                break;
            case Action.Equip2:
                break;
            case Action.Equip3:
                break;
            case Action.MoveDirectional:
                controller.statistics["Movement Input"].Set(Vector2.zero);
                break;
            case Action.MoveDirectionalX:
                input = controller.statistics["Movement Input"].Get<Vector2>();
                input.x = 0;
                controller.statistics["Movement Input"].Set(input);
                break;
            case Action.MoveDirectionalY:
                input = controller.statistics["Movement Input"].Get<Vector2>();
                input.y = 0;
                controller.statistics["Movement Input"].Set(input);
                break;
            case Action.MoveTargeted:
                controller.statistics["Movement Input"].Set(Vector2.zero);
                break;
            case Action.AimDirectional:
                //controller.statistics["Aim Input"].Set(Vector2.zero);
                // Aiming persists past input, so that the last aimed direction is always maintained.
                break;
            case Action.AimDirectionalX:
                //input = controller.statistics["Aim Input"].Get<Vector2>();
                //input.y = 0;
                //controller.statistics["Aim Input"].Set(input);
                // Aiming persists past input, so that the last aimed direction is always maintained.
                break;
            case Action.AimDirectionalY:
                //input = controller.statistics["Aim Input"].Get<Vector2>();
                //input.y = 0;
                //controller.statistics["Aim Input"].Set(input);
                // Aiming persists past input, so that the last aimed direction is always maintained.
                break;
            case Action.AimTargeted:
                //controller.statistics["Aim Input"].Set(Vector2.zero);
                // Aiming persists past input, so that the last aimed direction is always maintained.
                break;
            case Action.ZoomIn:
                break;
            case Action.ZoomOut:
                break;
            case Action.Zoom:
                break;
            case Action.ToggleView:
                break;
            case Action.Guide:
                break;
            case Action.Menu:
                break;
            case Action.Statistics:
                break;
        }
    }
}
