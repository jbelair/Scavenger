using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GuideCircleSequenceKey
{
    public string name;
    public float duration;
    public float durationCurrent;
    public float radiusStart;
    public float radiusEnd;
    public Gradient line;
    public Gradient fill;
    public Gradient label;
    public bool displayLabel;
}

[RequireComponent(typeof(GuideCircle))]
public class GuideCircleSequencer : MonoBehaviour
{
    public enum Format { Clamp, Wrap };

    public GuideCircle guide;
    public Format format = Format.Clamp;
    public List<GuideCircleSequenceKey> sequence;
    public int index = 0;

    // Use this for initialization
    void Start()
    {
        if (!guide)
            guide = GetComponent<GuideCircle>();
    }

    // Update is called once per frame
    void Update()
    {
        GuideCircleSequenceKey active = sequence[index];
        active.durationCurrent += Time.deltaTime;
        
        if (active.durationCurrent > active.duration)
        {
            int lastIndex = index;
            switch(format)
            {
                case Format.Clamp:
                    index = Mathf.Min(index + 1, sequence.Count - 1);
                    break;
                case Format.Wrap:
                    index = (index + 1) % sequence.Count;
                    break;
            }

            if (lastIndex != index)
            {
                // Remember the last key for a bit
                GuideCircleSequenceKey last = active;
                last.durationCurrent = 0;
                // Set the new active key
                active = sequence[index];
                // Make sure to carry over the extra time
                active.durationCurrent = last.durationCurrent - last.duration;
            }
        }

        float percent = active.durationCurrent / active.duration;
        // Adjust radius
        guide.radius = Mathf.Lerp(active.radiusStart, active.radiusEnd, percent);
        guide.lineColour = active.line.Evaluate(percent);
        guide.fillColour = active.fill.Evaluate(percent);
        guide.labelColour = active.label.Evaluate(percent);
        // Update guide
        guide.Adjust();
        guide.label.gameObject.SetActive(active.displayLabel);
    }
}
