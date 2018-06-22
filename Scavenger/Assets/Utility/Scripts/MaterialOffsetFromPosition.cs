using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialOffsetFromPosition : MonoBehaviour
{
    public Material material;
    public Vector2 scale = Vector2.one;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = ((Vector2)transform.position - Vector2.zero);
        offset.Scale(scale);
        material.mainTextureOffset = offset;
    }
}
