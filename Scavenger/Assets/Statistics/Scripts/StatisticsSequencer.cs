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
    }

    public void Update()
    {
        switch(format)
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

        float percent = Mathf.Clamp(durationCurrent / duration, 0f, 1f);
        int lastIndex = index;

        if (sequence.Count == 1 || sequence[1].percent > percent)
        {
            index = 0;
        }
        else
        {
            for (int i = 1; i < sequence.Count; i++)
            {
                if (sequence[i].percent > percent)
                {
                    index = i - 1;
                }
                else if (i == sequence.Count - 1)
                {
                    index = i;
                }
            }
        }

        if (lastIndex != index)
        {
            foreach(StatisticUEI stat in sequence[index].format)
            {
                Debug.Log(stat.name + " " + statistics.Has(stat.name));
                statistics[stat.name] = stat.Initialise();
            }
        }
    }
}
