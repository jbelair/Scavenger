using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TMProSystemCoordinate : MonoBehaviour
{
    public TextMeshProUGUI text;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        text.text = StringHelper.CoordinateName(Environment.SystemCoordinates);
    }
}
