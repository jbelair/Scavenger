using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ConstellationLineRenderer : MonoBehaviour
{
    public LineRenderer line;
    public float delay = 1;
    public float delayCurrent = 0;
    public float duration = 1;
    public float durationCurrent = 0;
    public Color startColourStart;
    public Color startColourEnd;
    public Color endColourStart;
    public Color endColourEnd;
    public Transform[] points;

    // Use this for initialization
    void Start()
    {
        if (!line)
            line = GetComponent<LineRenderer>();

        line.positionCount =points.Length;

        for(int i = 0; i < points.Length; i++)
        {
            line.SetPosition(i, points[i].position);   
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (delayCurrent >= delay)
        {
            float percentage = durationCurrent / duration;
            if (durationCurrent > duration)
                percentage = 2f - percentage;
            durationCurrent += Time.deltaTime;

            line.startColor = Color.Lerp(startColourStart, endColourStart, percentage);
            line.endColor = Color.Lerp(startColourEnd, endColourEnd, percentage);

            if (durationCurrent > duration * 2)
                Destroy(gameObject);
        }
        else
        {
            delayCurrent += Time.deltaTime;
        }
    }
}
