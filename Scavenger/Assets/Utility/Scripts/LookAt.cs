using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public bool lookAtParent = false;
    public Transform target;

    // Use this for initialization
    void Start()
    {
        if (lookAtParent)
            target = transform.parent;

        transform.LookAt(target, Vector3.forward);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (lookAtParent)
            target = transform.parent;

        transform.LookAt(target, Vector3.forward);
    }
}
