using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class WidgetSkill : MonoBehaviour
{
    public Skill definition;

    public Image icon;
    public TextMeshProUGUI nameWidget;
    public TextMeshProUGUI value;
    public WidgetSchemeImageColour background;

    // Use this for initialization
    internal virtual void Start()
    {
        Set();
    }

    public virtual void Set()
    {
        if (definition != null && definition.name != "")
        {
            bool unlocked = PlayerSave.saves[0].Get("unlocked skills").value.Contains(definition.name + " ");
            bool discovered = PlayerSave.saves[0].Get("discovered skills").value.Contains(definition.name + " ");
            string disabled = "<color=#" + ColorUtility.ToHtmlStringRGB(Schemes.Scheme("disabled").colour) + ">???";

            Scheme iconScheme = Schemes.Scheme(definition.icon);
            if (!iconScheme.symbol)
            {
                iconScheme = Schemes.Scheme("disabled");
            }
            icon.sprite = iconScheme.symbol;
            icon.color = iconScheme.colour;

            nameWidget.SetText(discovered ? Literals.active[definition.name] : disabled);
            value.SetText(discovered ? definition.value.ToString() : disabled);
            background.scheme = unlocked ? StringHelper.RarityIntToString(discovered ? definition.oneIn : 1) : "background";
            background.Set();
        }
        else
        {
            Scheme iconScheme = Schemes.Scheme("disabled");
            icon.sprite = iconScheme.symbol;
            icon.color = iconScheme.colour;

            nameWidget.SetText("");
            value.SetText("");
            background.Set();
        }
    }
}
