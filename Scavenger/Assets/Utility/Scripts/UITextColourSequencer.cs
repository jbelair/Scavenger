using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UITextColourSequencer : MonoBehaviour
{
    public Text text;

    public ColourSequencer sequence;

    void Awake()
    {
        text = GetComponent<Text>();
    }

    // Use this for initialization
    void Start()
    {
        text.color = sequence.Start();
    }

    // Update is called once per frame
    void Update()
    {
        text.color = sequence.Update();
    }
}
