using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WidgetLabel : MonoBehaviour
{
    public TextMeshProUGUI label;

    public TextMeshProUGUI[] text;
    public RectTransform target;

    // Use this for initialization
    void Start()
    {
        text = GetComponentsInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Add(string text)
    {
        TextMeshProUGUI inst = Instantiate(label, target);
        inst.SetText(text);
    }
}
