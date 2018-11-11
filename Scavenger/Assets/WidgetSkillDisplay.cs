using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WidgetSkillDisplay : WidgetSkill
{
    public static string set = "skill 0";
    
    public int index;
    public bool isSelectingCurrentSkill = true;

    public RectTransform self;
    public WidgetSkillGrid grid;

    // Update is called once per frame
    void Update()
    {

    }

    public override void Set()
    {
        base.Set();
    }

    public void Select()
    {
        grid.Set(index);

        if (isSelectingCurrentSkill)
        {
            PlayerSave.Active().Add(set, "Skill", JsonUtility.ToJson(definition));

            Players.players[0].statistics[set + " value"].Set(definition.value);
            if (Players.players[0].statistics[set] != null)
                Players.players[0].statistics[set].Set(definition);
            else
                Players.players[0].statistics[set] = new Statistic(set, Statistic.ValueType.Object, definition);
        }

        foreach(WidgetSkillSelect skill in WidgetSkillSelect.skills)
        {
            if (skill.skillBinding == set)
            {
                skill.definition = definition;
                skill.Set();
            }
        }
    }
}
