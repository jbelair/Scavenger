using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFitTransform : MonoBehaviour
{
    public Transform bounds;
    public float speed = 1000;
    public float minDistance = 300;
    public float maxDistance = 1000;

    public int children = 0;
    public float distance;
    public Vector3 position;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDistance = Vector3.zero;
        Vector3 target = Vector3.zero;

        for(int i = 0; i < bounds.childCount; i++)
        {
            Transform child = bounds.GetChild(i);
            if (!child.gameObject.name.Contains("Orbit"))
            {
                if (child.position.magnitude > targetDistance.magnitude)
                    targetDistance = child.position;

                target += child.position;
            }
        }

        if (bounds.childCount > 0)
            target /= bounds.childCount;

        distance = targetDistance.magnitude * 2;
        children = bounds.childCount;

        transform.position = Vector3.MoveTowards(transform.position, Vector3.zero + transform.forward * -Mathf.Max(minDistance, Mathf.Min(maxDistance, targetDistance.magnitude * 2)), speed * Time.deltaTime);
    }
}
