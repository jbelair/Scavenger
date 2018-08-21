using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GravityLenseSequencerKey
{
    public string name;
    public float duration;
    public float durationCurrent;
    public float radiusStart;
    public float radiusEnd;
    public float eventHorizonStart;
    public float eventHorizonEnd;
    public float sharpnessStart;
    public float sharpnessEnd;
    [ColorUsage(false, true)]
    public Color singularityStart;
    [ColorUsage(false, true)]
    public Color singularityEnd;
}

public class GravityLenseSequencer : MonoBehaviour
{
    public enum Format { Clamp, Wrap };

    public Camera postProcessing;
    public GravityLenseImageEffect effect;
    public Format format = Format.Clamp;
    public List<GravityLenseSequencerKey> sequence;
    public int index = 0;

    // Use this for initialization
    void Start()
    {
        if (!postProcessing)
            postProcessing = Camera.main;

        postProcessing.depthTextureMode = DepthTextureMode.Depth;

        effect = postProcessing.gameObject.AddComponent<GravityLenseImageEffect>();
        effect.origin = transform;
    }

    // Update is called once per frame
    void Update()
    {
        GravityLenseSequencerKey active = sequence[index];
        active.durationCurrent += Time.deltaTime;

        if (active.durationCurrent > active.duration)
        {
            int lastIndex = index;
            switch (format)
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
                GravityLenseSequencerKey last = active;
                // Set the new active key
                active = sequence[index];
                // Make sure to carry over the extra time
                active.durationCurrent = last.durationCurrent - last.duration;
                last.durationCurrent = 0;
            }
        }

        float percent = active.durationCurrent / active.duration;
        // Adjust radius
        effect.radius = Mathf.Lerp(active.radiusStart, active.radiusEnd, percent);
        // Adjust event horizon
        effect.eventHorizon = Mathf.Lerp(active.eventHorizonStart, active.eventHorizonEnd, percent);
        effect.sharpness = Mathf.Lerp(active.sharpnessStart, active.sharpnessEnd, percent);
        effect.singularity = Color.Lerp(active.singularityStart, active.singularityEnd, percent);
    }
}
