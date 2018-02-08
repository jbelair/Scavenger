using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class SkillSequence
{
    public enum SequenceFormat { Instant, SequentialCharge, GuaranteedCharge, InterruptibleChannel, NonInterruptibleChannel, Delay };

    public string name;
    public SequenceFormat format;
    public float duration;
    public float durationCurrent;
    private bool dirty;
    private float perc;
    public float percentage { get { if (dirty) perc = durationCurrent / duration; return perc; } }
    public List<SkillSequenceKey> keys = new List<SkillSequenceKey>();

    private float timeOfStart = -1;

    public GameObject[] Evaluate()
    {
        List<GameObject> instantiables = new List<GameObject>();

        if (timeOfStart == -1)
        {
            timeOfStart = Time.time;
            if (keys[0].percentage == 0)
                instantiables.AddRange(keys[0].instantiables);
        }
        else
        {
            durationCurrent = Time.time - timeOfStart;
            
            for (int i = 0; i < keys.Count; i++)
            {
                if (keys[i].percentage >= percentage - 0.05f && keys[i].percentage <= percentage + 0.05f)
                {
                    instantiables.AddRange(keys[i].instantiables);
                }
            }
        }

        return instantiables.ToArray();
    }
}
