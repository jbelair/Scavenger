using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSequencer : MonoBehaviour
{
    public Material material;
    public FloatSequencer kelvinSequence;
    public FloatSequencer kelvinRangeSequence;

    private void Awake()
    {
        if (!material)
            material = GetComponentInParent<MeshRenderer>().sharedMaterial;
    }

    // Use this for initialization
    void Start()
    {
        material.SetFloat("_Kelvin", kelvinSequence.Start());
        material.SetFloat("_KelvinRange", kelvinRangeSequence.Start());
    }

    // Update is called once per frame
    void Update()
    {
        material.SetFloat("_Kelvin", kelvinSequence.Update());
        material.SetFloat("_KelvinRange", kelvinRangeSequence.Update());
    }
}
