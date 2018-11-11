using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WidgetMatterDisplay : MonoBehaviour
{
    public StatisticUEI matterUEI;
    public Statistic matter;
    public StatisticUEI shipValueUEI;
    public Statistic shipValue;
    public StatisticUEI skill0ValueUEI;
    public Statistic skill0Value;
    public StatisticUEI skill1ValueUEI;
    public Statistic skill1Value;
    public StatisticUEI skill2ValueUEI;
    public Statistic skill2Value;
    public StatisticUEI skill3ValueUEI;
    public Statistic skill3Value;
    public TextMeshProUGUI text;

    // Use this for initialization
    void Initialise()
    {
        if (Players.players[0] && Players.players[0].statistics && Players.players[0].statistics.Has("matter") && Players.players[0].statistics.Has("ship value"))
        {
            matter = Players.players[0].statistics["matter"];
            matterUEI = matter;

            shipValue = Players.players[0].statistics["ship value"];
            shipValueUEI = shipValue;

            skill0Value = Players.players[0].statistics["skill 0 value"];
            skill0ValueUEI = skill0Value;

            skill1Value = Players.players[0].statistics["skill 1 value"];
            skill1ValueUEI = skill1Value;

            skill2Value = Players.players[0].statistics["skill 2 value"];
            skill2ValueUEI = skill2Value;

            skill3Value = Players.players[0].statistics["skill 3 value"];
            skill3ValueUEI = skill3Value;

            Set();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (matter == null)
            Initialise();
        else
            Set();
    }

    void Set()
    {
        string hexNormal = ColorUtility.ToHtmlStringRGBA(Schemes.Scheme("default").colour);
        string hexNegative = ColorUtility.ToHtmlStringRGBA(Schemes.Scheme("negative").colour);
        int iMatter = matter.Get<int>();
        int iValue = shipValue.Get<int>() + skill0Value.Get<int>() + skill1Value.Get<int>() + skill2Value.Get<int>() + skill3Value.Get<int>();
        text.SetText("<color=#" + hexNormal + ">" + iMatter + "<color=#" + hexNegative + ">-" + iValue + "<color=#" + hexNormal + ">=" + (iMatter - iValue));
    }
}
