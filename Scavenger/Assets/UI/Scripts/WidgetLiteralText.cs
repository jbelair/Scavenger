using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class WidgetLiteralText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public bool setFromText = true;
    public string startText = "";

    private void Start()
    {
        if (setFromText)
        {
            startText = text.text;
            Set(text.text);
        }
        else
        {
            Set(startText);
        }
    }

    public void Set(string literal)
    {
        text.SetText(Literals.active[literal]);
    }
}
