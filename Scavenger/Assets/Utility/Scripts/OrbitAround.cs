using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitAround : MonoBehaviour
{
    public enum Axis { x, y, z };

    public Transform center;
    public float speed = 1;
    public float axisOffset;
    public Axis axis = Axis.z;

    [Header("Diagnostics")]
    public Vector3 goal;
    public float offset;
    public float theta;

    // Use this for initialization
    void Start()
    {
        Vector3 delta = center.position - transform.position;

        offset = delta.magnitude;
        theta = Mathf.Atan2(delta.y, delta.x) + Mathf.PI;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, goal, speed);

        theta += speed * Time.deltaTime * Mathf.PI * 2;
        if (axis == Axis.z)
            goal = center.position + new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), axisOffset) * offset;
        //else if (axis == Axis.)
    }
}
