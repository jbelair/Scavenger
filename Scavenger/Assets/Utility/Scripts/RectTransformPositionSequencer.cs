using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class RectTransformPositionSequencer : MonoBehaviour
{

    public RectTransform rect;
    public Vector3Sequencer positions;

    void Awake()
    {
        if (!rect)
            rect = GetComponent<RectTransform>();
    }

    // Use this for initialization
    void Start()
    {
        rect.localPosition = positions.Start();
    }

    // Update is called once per frame
    void Update()
    {
        rect.localPosition = positions.Update();
    }
}
