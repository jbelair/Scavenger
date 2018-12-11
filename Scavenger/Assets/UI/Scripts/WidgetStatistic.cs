using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WidgetStatistic : MonoBehaviour
{
    public string statistic_ref;
    public bool isRefFromStartingText = false;

    public TextMeshProUGUI value;
    public GameObject label;
    public TextMeshProUGUI labelText;
    public bool isValueFromRef = false;
    public bool isTextColourFromScheme = true;
    public Image icon;
    public bool isIconFromScheme = true;
    public bool isIconColourFromScheme = true;

    public void Initialise()
    {
        if (isRefFromStartingText)
            statistic_ref = value.text;

        //Debug.Log(statistic_ref);
        if (labelText && Literals.active.ContainsKey(statistic_ref))
            labelText.SetText(Literals.active[statistic_ref]);
        if (isValueFromRef)
            value.SetText(labelText.text);

        if (isTextColourFromScheme || isIconColourFromScheme || isIconFromScheme)
        {
            Scheme scheme = Schemes.Scheme(statistic_ref);
            if (isTextColourFromScheme)
                value.color = scheme.colour;
            if (isIconFromScheme)
                icon.sprite = scheme.symbol;
            if (isIconColourFromScheme)
                icon.color = scheme.colour;
        }
    }

    private void Start()
    {
        Initialise();
    }
}
