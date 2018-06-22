using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererWidthScaler : MonoBehaviour
{
    [System.Serializable]
    public class WidthKey
    {
        public float width;
        public float distance;
    }

    public LineRenderer line;

    public WidthKey minimum;
    public WidthKey maximum;

    // Use this for initialization
    void Start()
    {
        if (!line)
            line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (line)
        {
            float distance = ((Camera.main.transform.position - transform.position).magnitude - minimum.distance) / (maximum.distance - minimum.distance);
            line.endWidth = line.startWidth = Mathf.Lerp(minimum.width, maximum.width, distance);
        }
    }
}
