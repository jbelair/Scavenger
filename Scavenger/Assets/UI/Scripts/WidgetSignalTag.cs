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
        Scheme scheme = Schemes.Scheme(tag);

        Debug.Log(scheme.name);

        if (label)
            label.text = Literals.active[scheme.name];

        if (icon)
        {
            icon.sprite = scheme.symbol;
            icon.color = scheme.colour;
        }
    }
}
