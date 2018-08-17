using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer)), ExecuteInEditMode]
public class LineRendererSpiral : MonoBehaviour
{
    public LineRenderer line;
    public Transform center;
    public float revolutions = 2;
    public int segments = 32;

    public Vector3 lastCenter;

    // Use this for initialization
    void Start()
    {
        Set();
    }

    // Update is called once per frame
    void Update()
    {
        if (lastCenter != center.position)
            Set();
    }

    [ExposeMethodInEditor]
    public void Set()
    {
        if (!line)
            line = GetComponent<LineRenderer>();

        if (!line)
            return;

        lastCenter = center.position;

        Vector2 delta = line.transform.position - lastCenter;
        float distance = delta.magnitude;
        float theta = (Mathf.PI * 2 * revolutions) / segments;
        float offset = Mathf.Atan2(delta.y, delta.x);

        line.positionCount = segments+1;

        for (float i = 0; i <= segments; i++)
        {
            line.SetPosition((int)i, lastCenter + new Vector3(Mathf.Cos(offset + theta * i) * distance * (i / segments), Mathf.Sin(offset + theta * i) * distance * (i / segments), 0));
        }
    }
}
