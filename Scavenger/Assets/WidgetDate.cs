using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WidgetDate : MonoBehaviour
{
    public int startYear = 2147;
    public int startMonth = 12;
    public int startDay = 12;
    public int startHour = 0;
    public int startMinute = 0;
    public int startSecond = 0;
    public TextMeshProUGUI text;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int second = (startSecond + Mathf.FloorToInt((Environment.environmentTime + Environment.environmentTimeCounter) * 24f * 60f * 60f)) % 60;
        int minute = (startMinute + Mathf.FloorToInt((Environment.environmentTime + Environment.environmentTimeCounter) * 24f * 60f)) % 60;
        int hour = (startHour + Mathf.FloorToInt((Environment.environmentTime + Environment.environmentTimeCounter) * 24f)) % 24;
        int day = (startDay + Mathf.FloorToInt((Environment.environmentTime + Environment.environmentTimeCounter))) % 28 + 1;
        int month = (startMonth + Mathf.FloorToInt((Environment.environmentTime + Environment.environmentTimeCounter) / 28f)) % 13 + 1;
        int year = startYear + Mathf.FloorToInt((Environment.environmentTime + Environment.environmentTimeCounter) / 28f / 13f);

        text.text = year + ".";

        if (month < 10)
            text.text += "0" + month;
        else
            text.text += month;

        text.text += ".";

        if (day < 10)
            text.text += "0" + day;
        else
            text.text += day;

        text.text += ".";

        if (hour < 10)
            text.text += "0" + hour;
        else if (hour < 24)
            text.text += hour;
        else
            text.text += "00";

        text.text += ":";

        if (minute < 10)
            text.text += "0" + minute;
        else if (minute < 60)
            text.text += minute;
        else
            text.text += "00";

        text.text += ".";

        if (second < 10)
            text.text += "0" + second;
        else if (second < 100)
            text.text += second;
        else
            text.text += "00";
    }
}
