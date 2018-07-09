using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer)), ExecuteInEditMode]
public class LineRendererCircle : MonoBehaviour
{
    public LineRenderer line;

    public int pointsOnCircle = 32;
    public float distanceBetweenPoints = 122f;
    public bool pointsAuto = false;
    public float radius = 100;

	// Use this for initialization
	public virtual void Start ()
    {
        if (!line)
            line = GetComponent<LineRenderer>();

        SetCircle();
	}
	
	// Update is called once per frame
	public virtual void Update ()
    {
		
	}

    [ExposeMethodInEditor]
    public void SetCircle()
    {
        if (pointsAuto)
            pointsOnCircle = Mathf.FloorToInt(2f * Mathf.PI * radius / distanceBetweenPoints);

        line.positionCount = pointsOnCircle;

        float theta = Mathf.PI * 2.0f / pointsOnCircle;
        for (int i = 0; i < pointsOnCircle; i++)
        {
            float x = Mathf.Cos(theta * i) * radius;
            float y = Mathf.Sin(theta * i) * radius;
            Vector3 position = new Vector3(x, y, 0f);
            line.SetPosition(i, position);
        }
    }
}
