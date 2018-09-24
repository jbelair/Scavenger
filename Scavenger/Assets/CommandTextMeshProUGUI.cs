using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CommandTextMeshProUGUI : MonoBehaviour
{
    public TextMeshProUGUI text;

    // Use this for initialization
    void Start()
    {
        text.text = StringHelper.CommandParse(text.text);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
