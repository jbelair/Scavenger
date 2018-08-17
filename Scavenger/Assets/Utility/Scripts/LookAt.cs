using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform target;

    // Use this for initialization
    void Start()
    {
        transform.LookAt(target, Vector3.forward);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(target, Vector3.forward);
    }
}
