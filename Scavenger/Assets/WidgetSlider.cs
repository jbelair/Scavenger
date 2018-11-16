using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WidgetSlider : MonoBehaviour
{
    public string schemeBackground = "default";
    public string schemeFill = "default";
    public string schemeHandle = "default";

    public Slider slider;
    public Image background;
    public Image fill;
    public Image handle;
    public TextMeshProUGUI label;
    public TextMeshProUGUI labelLow;
    public TextMeshProUGUI labelHigh;
    public TextMeshProUGUI labelValue;

    void Initialise()
    {
        labelLow.SetText(slider.minValue.ToString());
        labelHigh.SetText(slider.maxValue.ToString());

        Scheme sB = Schemes.Scheme(schemeBackground);
        Scheme sF = Schemes.Scheme(schemeFill);
        Scheme sH = Schemes.Scheme(schemeHandle);

        background.color = sB.colour;
        fill.color = sF.colour;
        handle.color = sH.colour;
    }

    // Use this for initialization
    void Start()
    {
        Initialise();
    }

    // Update is called once per frame
    void Update()
    {
        if (labelValue)
            labelValue.SetText(slider.value.RoundTo(1).ToString());
    }
}
