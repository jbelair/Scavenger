using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WidgetMatterDisplay : MonoBehaviour
{
    public StatisticUEI matterUEI;
    public Statistic matter;
    public StatisticUEI valueUEI;
    public Statistic value;
    public TextMeshProUGUI text;

    // Use this for initialization
    void Initialise()
    {
        if (Players.players[0] && Players.players[0].statistics && Players.players[0].statistics.Has("matter") && Players.players[0].statistics.Has("value"))
        {
            matter = Players.players[0].statistics["matter"];
            matterUEI = matter;

            value = Players.players[0].statistics["value"];
            valueUEI = value;

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
        int iValue = value.Get<int>();
        text.SetText("<color=#" + hexNormal + ">" + iMatter + "<color=#" + hexNegative + ">-" + iValue + "<color=#" + hexNormal + ">=" + (iMatter - iValue));
    }
}
