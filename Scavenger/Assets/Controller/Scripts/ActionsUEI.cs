using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class ActionsUEI : MonoBehaviour
{
    public List<ControllerAction> actions;

    //public TextAsset file;

    private void Start()
    {
        foreach(ControllerAction action in actions)
        {
            if (!ControllerAction.Actions.ContainsKey(action.action))
                ControllerAction.Actions.Add(action.action, action);
        }
    }
}

// PRIVATE VOID START
// INSERT BEFORE foreach(ControllerAction action in actions)
//if (file)
//{
//    using (StringReader reader = new StringReader(file.text))
//    {
//        string line = reader.ReadLine();
//        // Any number of spaces may exist anywhere, they will be removed before it is processed
//        line = StringHelper.RemoveSpaces(line);
//        // Ini file line format CATEGORY.PROPERTY=TARGET.METHOD:START
//        // CATEGORY = The category of the ini file this statement belongs to
//        // PROPERTY = The categorized property this ini statement defines
//        // TARGET = The target as one of a limited selection of predefined ones
//        // METHOD = The method to call on the target
//        // START = Boolean value that defines whether to call the method on start or on end.
//        string[] split = line.Split('.', '=', ':');
//        if (split[0] == "Input")
//        {
//            ControllerAction action;
//            if (ControllerAction.Actions.ContainsKey(split[1]))
//                action = ControllerAction.Actions[split[1]];
//            else
//            {
//                action = new ControllerAction();
//                action.action = split[1];
//            }

//            if (split[3] == "Player")
//            {
//                // ADD ALL ACTIVE PLAYERS .METHOD TO THE ACTION
//                // foreach(PlayerUEI player in Players.Active) { player }
//                foreach(PlayerUEI player in PlayerUEI.Active)
//                {
//                    Type playerType = player.GetType();
//                    MethodInfo method = playerType.GetMethod(split[3]);
//                    if (method != null)
//                    {
//                        Boolean start = true;

//                        Boolean.TryParse(split[4], out start);

//                        if (start)
//                            action.start.AddListener(method.Invoke(player));
//                    }
//                    //    method.Invoke(handler, new object[] { });
//                }
//            }
//            else if (split[3] == "UI")
//            {
//                // ADD UI MASTER INPUT MODULE .METHOD TO THE ACTION
//            }
//            else if (split[3] == "Game")
//            {
//                // ADD GAME .METHOD TO THE ACTION
//            }
//            else
//            {
//                // DON'T CARE; JUST HERE TO SAY THAT
//            }
//        }
//    }
//}
