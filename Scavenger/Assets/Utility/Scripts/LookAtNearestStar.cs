using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtNearestStar : MonoBehaviour
{
    public Vector3 originalPosition;
    public bool lookAtParent = true;
    public int nearestStars = 1;
    public List<EnvironmentBasedStar> stars;
    public List<Transform> transforms;

    // Use this for initialization
    void Start()
    {
        originalPosition = transform.position;

        if (lookAtParent)
            transforms.Add(transform.parent);

        stars = new List<EnvironmentBasedStar>();
        stars.AddRange(GameObject.FindObjectsOfType< EnvironmentBasedStar>());
        
        for (int i = 0; i < stars.Count; i++)
        {
            for (int j = i; j < stars.Count; j++)
            {
                if (Vector3.Distance(transform.position, stars[i].transform.position) > Vector3.Distance(transform.position, stars[j].transform.position))
                {
                    EnvironmentBasedStar tmp = stars[i];
                    stars[i] = stars[j];
                    stars[j] = tmp;
                }
            }
        }

        for (int i = 0; i < nearestStars; i++)
        {
            transforms.Add(stars[i].transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = new Vector3();

        foreach (Transform transform in transforms)
        {
            position += transform.position;
        }
        position /= transforms.Count;

        float distance = Vector3.Distance(transform.parent.position, position);
        transform.position = originalPosition + (transform.position - originalPosition).normalized * distance * 0.25f + (transform.parent.position - position).normalized * distance * 0.05f;
        transform.LookAt(position, Vector3.forward);
    }
}
