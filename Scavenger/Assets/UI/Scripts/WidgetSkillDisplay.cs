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
    public bool isUnlocked = true;
    public bool isDiscovered = true;

    public RectTransform self;
    public WidgetSkillGrid grid;

    List<Graphic> graphics;

    // Update is called once per frame
    void LateUpdate()
    {
        if (graphics == null)
        {
            graphics = new List<Graphic>(GetComponentsInChildren<Graphic>());
        }

        foreach (Graphic graphic in graphics)
        {
            if (!isUnlocked)
                graphic.color = graphic.color.A(0.5f);
        }
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
            PlayerSave.Active.Add(set, "Skill", JsonUtility.ToJson(definition));

            Players.players[0].statistics[set + " value"].Set(definition.value);
            Players.players[0].statistics[set].Set(definition);
        }

        foreach (WidgetSkillSelect skill in WidgetSkillSelect.skills)
        {
            if (skill.skillBinding == set)
            {
                skill.definition = definition;
                skill.Set();
            }
        }
    }
}
