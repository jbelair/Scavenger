using System;
using UnityEngine;

public class FollowTargetByTag : MonoBehaviour
{
    public string tag;
    public Transform target;
    public Vector3 offset = new Vector3(0f, 7.5f, 0f);

    private void LateUpdate()
    {
        if (target)
            transform.position = target.position + offset;
        else
            target = GameObject.FindGameObjectWithTag(tag).transform;
    }
}
