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
        WidgetScheme.SchemeContainer scheme = WidgetScheme.active.Scheme(tag);
        if (label)
            label.text = scheme.name;

        if (icon)
        {
            icon.sprite = scheme.symbol;
            icon.color = scheme.colour;
        }
    }
}
