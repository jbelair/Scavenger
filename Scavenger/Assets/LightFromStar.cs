﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light)), ExecuteInEditMode]
public class LightFromStar : MonoBehaviour
{
    public Material material;
    public Light light;
    public Texture2D texture;

    private void Awake()
    {
        if (!material)
        {
            material = GetComponentInParent<MeshRenderer>().sharedMaterial;
            texture = (material.GetTexture("_Emissive") as Texture2D);
        }

        if (!light)
            light = GetComponent<Light>();

        if (!texture)
            texture = (material.GetTexture("_Emissive") as Texture2D);
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        light.color = texture.GetPixelBilinear(((material.GetFloat("_Kelvin") + material.GetFloat("_KelvinRange") / 2) / material.GetFloat("_KelvinMax")), 0);
        light.intensity = new Vector3(light.color.r, light.color.g, light.color.b).magnitude * 8;
	}
}
