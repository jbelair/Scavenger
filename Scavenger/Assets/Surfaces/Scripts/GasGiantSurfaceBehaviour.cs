using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class GasGiantSurfaceBehaviour : MonoBehaviour
{
    public MeshRenderer mesh;



    private void Awake()
    {
        if (!mesh)
            mesh = GetComponentInParent<MeshRenderer>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
