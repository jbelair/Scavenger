using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WidgetSearch : MonoBehaviour
{
    public TMP_InputField field;
    public TextMeshProUGUI recommendation;

    public string startText;
    public string lastText;

    // Use this for initialization
    void Start()
    {
        if (SystemsFilter.active != null && SystemsFilter.active.filterTags != "")
            field.text = SystemsFilter.active.filterTags;

        if (SystemsFilter.active != null && SystemsFilter.lastFilterTags != "")
            field.text = SystemsFilter.lastFilterTags;

        startText = field.text;
    }

    // Update is called once per frame
    void Update()
    {
        List<string> recommendations = new List<string>();
        string[] split = field.text.Split(new string[] { ", " }, System.StringSplitOptions.RemoveEmptyEntries);

        string[] tags = new string[DungeonType.allTags.Length];

        for(int i = DungeonType.allTags.Length - 1; i >= 0; i--)
        {
            string str = PlayerPrefs.GetString("language");
            tags[i] = Literals.active[DungeonType.allTags[i]];
        }

        if (field.text != startText && split.Length > 0)
        {
            recommendations.AddRange(StringHelper.ClosestMatches(tags, split[split.Length - 1]));
        }
        else
        {
            field.text = "";
            recommendation.text = "...";
            return;
        }

        if (field.text != lastText && field.text != startText)
        {
            recommendation.text = recommendations[0];
            for (int i = 1; i < 5; i++)
            {
                recommendation.text += ", " + recommendations[i];
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (split.Length < 2)
                field.text = recommendations[0];
            else
            {
                field.text = split[0];

                for(int i = 1; i < split.Length - 1; i++)
                {
                    field.text += ", " + split[i];
                }

                field.text += ", " + recommendations[0];
            }

            field.text += ", ";
            field.MoveToEndOfLine(false, true);
            field.ForceLabelUpdate();
            //field.caretPosition = field.text.Length;
        }

        lastText = field.text;
    }
}
