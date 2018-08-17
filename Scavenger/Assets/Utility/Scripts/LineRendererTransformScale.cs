using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererTransformScale : MonoBehaviour
{
    public LineRenderer line;

    public Vector3[] linePoints;
    public float linePointsMultiplier;
    public float lineCurveMultiplier;

    // Use this for initialization
    void Start()
    {
        linePoints = new Vector3[line.positionCount];

        for (int i = 0; i < line.positionCount; i++)
        {
            linePoints[i] = line.GetPosition(i);
        }

        lineCurveMultiplier = line.widthMultiplier;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        line.widthMultiplier = lineCurveMultiplier * transform.lossyScale.x;

        for(int i = 0; i < line.positionCount; i++)
        {
            line.SetPosition(i, linePoints[i] * linePointsMultiplier * transform.lossyScale.x);
        }
    }
}
