using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WidgetSkillSelect : WidgetSkill
{
    public static List<WidgetSkillSelect> skills = new List<WidgetSkillSelect>();

    public string skillBinding = "skill 0";

    internal override void Start()
    {
        if (Players.players.Count > 0 && Players.players[0].statistics && Players.players[0].statistics.Has(skillBinding))
        {
            object obj = Players.players[0].statistics[skillBinding].Get<object>();
            if (obj != null)
            {
                definition = Players.players[0].statistics[skillBinding].Get<Skill>();
            }
        }
        base.Start();

        skills.Add(this);
    }

    public override void Set()
    {
        base.Set();
    }

    public void Select()
    {
        WidgetSkillDisplay.set = skillBinding;
    }

    private void OnDestroy()
    {
        skills.Remove(this);
    }
}
