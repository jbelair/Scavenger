using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WidgetSignalTag : MonoBehaviour
{
    new public string tag;
    public TextMeshProUGUI label;
    public Image icon;

    // Use this for initialization
    void Start()
    {
        WidgetScheme.SchemeContainer scheme = WidgetScheme.Scheme(tag);

        if (label)
            label.text = Literals.literals[PlayerPrefs.GetString("language")][scheme.name];

        if (icon)
        {
            icon.sprite = scheme.symbol;
            icon.color = scheme.colour;
        }
    }
}
