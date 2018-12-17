using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameValidator : MonoBehaviour
{
    public SwitchScene switchScene;
    /// <summary>
    /// This validator will be launched whenever an engage button is clicked
    /// As such it must ensure that the newly started game is correctly set up.
    /// Correct set up requires the following:
    /// A valid selected ship
    /// 4 valid selected skills
    /// 2 valid selected skins (ship and shield)
    /// </summary>
    public void Launch()
    {
        Player player = Players.players[0];
        int valid = 0;
        string invalid = Literals.active["validate_game_new"];
        if (player.statistics.Has("ship"))
            valid++;
        else
            invalid += Literals.active["validate_game_new_ship"];

        int skills = 0;
        if (player.statistics.Has("skill 0"))
        {
            valid++;
            skills++;
        }

        if (player.statistics.Has("skill 1"))
        {
            valid++;
            skills++;
        }

        if (player.statistics.Has("skill 2"))
        {
            valid++;
            skills++;
        }

        if (player.statistics.Has("skill 3"))
        {
            valid++;
            skills++;
        }

        if (skills != 4)
            invalid += Literals.active["validate_game_new_skills"];

        if (player.statistics.Has("ship skin"))
        {
            valid++;
            player.statistics["ship material"].Set(new Material(Materials.materials[Skins.Get(player.statistics["ship skin"]).skin]));
        }
        else
            invalid += Literals.active["validate_game_new_skin_ship"];

        if (player.statistics.Has("shield skin"))
        {
            valid++;
            player.statistics["shield material"].Set(new Material(Materials.materials[Skins.Get(player.statistics["shield skin"]).skin]));
        }
        else
            invalid += Literals.active["validate_game_new_skin_shield"];

        if (valid >= 7)
            switchScene.Switch("Game.Systems");
        else
        {
            WidgetMessage message = UIManager.active.Element("focus", UIManager.Layer.Mid, "widget message").GetComponent<WidgetMessage>();
            message.Set(invalid);
        }
    }
}
