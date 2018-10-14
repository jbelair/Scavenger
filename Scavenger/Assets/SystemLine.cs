using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemLine : MonoBehaviour
{
    public Linkable link;
    public WidgetSystem system;
    public WidgetWorldPosition position;
    public LineRenderer line;
    public float radius = 0.2f;

    public bool polling = true;
    public float update = 0.1f;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(Poll());
    }

    // Update is called once per frame
    IEnumerator Poll()
    {
        while (polling)
        {
            if (link.link)
            {
                if (!system)
                    system = link.link.GetComponentInChildren<WidgetSystem>();

                if (!position)
                    position = link.link.GetComponentInChildren<WidgetWorldPosition>();

                line.SetPosition(0, position.target.position.XY());
                line.SetPosition(1, position.target.position);
                line.colorGradient = new Gradient()
                {
                    alphaKeys = new GradientAlphaKey[]
                    {
                    new GradientAlphaKey(system.distanceFromCursor, 0),
                    new GradientAlphaKey(0, 1),
                    },
                    colorKeys = new GradientColorKey[]
                    {
                    new GradientColorKey(system.colour, 0)
                    }
                };
            }

            yield return new WaitForSeconds(update);
        }
    }
}
