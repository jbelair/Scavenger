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
            Scheme iconScheme = Schemes.Scheme(definition.icon);
            if (!iconScheme.symbol)
            {
                iconScheme = Schemes.Scheme("disabled");
            }
            icon.sprite = iconScheme.symbol;
            icon.color = iconScheme.colour;

            nameWidget.SetText(Literals.active[definition.name]);
            value.SetText(definition.value.ToString());
            background.scheme = StringHelper.RarityIntToString(definition.oneIn);
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
