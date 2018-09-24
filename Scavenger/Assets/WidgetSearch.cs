using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WidgetSearch : MonoBehaviour
{
    public TMP_InputField field;

    // Use this for initialization
    void Start()
    {
        if (SystemsFilter.active != null && SystemsFilter.active.filterTags != "")
            field.text = SystemsFilter.active.filterTags;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
