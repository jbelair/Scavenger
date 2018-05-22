using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(Statistics))]
public class StatisticsSequencer : MonoBehaviour
{
    public enum Format { Time, Distance, Displacement };

    public Statistics statistics;

    public Format format = Format.Distance;
    public float duration = 0;
    public float durationCurrent = 0;

    public Vector2 startPosition = Vector2.zero;
    private Vector2 lastPosition = Vector2.zero;

    public List<StatisticsSequenceKey> sequence;
    public int index = -1;

    public void Start()
    {
        if (!statistics)
            statistics = GetComponentInParent<Statistics>();

        startPosition = transform.position;

        index = -1;
    }

    public void Update()
    {
        switch (format)
        {
            case Format.Time:
                durationCurrent += Time.deltaTime;
                break;
            case Format.Distance:
                durationCurrent += (lastPosition - (Vector2)transform.position).magnitude;
                lastPosition = transform.position;
                break;
            case Format.Displacement:
                durationCurrent = (startPosition - (Vector2)transform.position).magnitude;
                break;
        }

        //float percent = Mathf.Clamp(durationCurrent / duration, 0f, 1f);
        int lastIndex = index;
        if (index == -1)
        {
            index = 0;
        }

        StatisticsSequenceKey active = sequence[index];
        //float percent = active.durationCurrent / active.duration;

        if (duration > 0 && durationCurrent >= duration)
        {
            //percent = 1;
        }
        else
        {
            active.durationCurrent += Time.deltaTime;

            if (active.durationCurrent > active.duration)
            {
                index = Mathf.Min(index + 1, sequence.Count - 1);
            }

            if (lastIndex != index)
            {
                // Remember the last key for a bit
                StatisticsSequenceKey last = active;
                // Set the new active key
                active = sequence[index];
                // Make sure to carry over the extra time
                active.durationCurrent = last.durationCurrent - last.duration;
                last.durationCurrent = 0;

                foreach (StatisticUEI stat in active.format)
                {
                    //Debug.Log(stat.name);
                    statistics[stat.name] = stat.Initialise();
                    statistics[stat.name].isDirty = true;
                }
            }
        }
    }
}
