using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ProjectileStatisticSequencer : MonoBehaviour
{
    public enum Format { Time, Distance, Displacement };

    public ProjectileUEI projectile;

    public Format format = Format.Distance;
    public float duration = 0;
    public float durationCurrent = 0;

    private Vector2 lastPosition = Vector2.zero;

    public List<ProjectileSequenceKey> sequences;
    public int index = -1;

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
                durationCurrent = (projectile["Owner"].Get<GameObject>().transform.position - transform.position).magnitude;
                break;
        }

        float percent = Mathf.Clamp(durationCurrent / duration, 0f, 1f);
        int lastIndex = index;

        if (sequences.Count == 1 || sequences[1].percent > percent)
        {
            index = 0;
        }
        else
        {
            for (int i = 1; i < sequences.Count; i++)
            {
                if (sequences[i].percent > percent)
                {
                    index = i - 1;
                }
                else if (i == sequences.Count - 1)
                {
                    index = i;
                }
            }
        }

        if (lastIndex != index)
        {
            foreach(StatisticUEI stat in sequences[index].format)
            {
                projectile[stat.name] = stat.Initialise();
            }
        }
    }
}
