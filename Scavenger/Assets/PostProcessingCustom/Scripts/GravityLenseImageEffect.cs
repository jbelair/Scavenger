using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GravityLenseImageEffect : MonoBehaviour
{
    private Shader shader;
    private Material material;

    [Range(0,float.MaxValue), Tooltip("Defines the radius of the gravity lensing.")]
    public float radius = 1;
    [Range(0,float.MaxValue), Tooltip("Defines the radius of the event horizon, where all lensing is turned to singularity colour.")]
    public float eventHorizon = 0;
    public float sharpness = 0;
    [ColorUsage(true, true, 0, 8, 1/8, 3), Tooltip("Defines the colour of the event horizon.")]
    public Color singularity;
    [Tooltip("Defines the origin of the gravity lense, in world space.")]
    public Transform origin;

    public void Start()
    {
        shader = Shader.Find("Hidden/Gravity Lensing");
        material = new Material(shader);
    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (origin && radius > 0)
        {
            Vector2 pos = new Vector2(Camera.main.WorldToScreenPoint(origin.transform.position).x / Camera.main.pixelWidth, Camera.main.WorldToScreenPoint(origin.transform.position).y / Camera.main.pixelHeight);

            material.SetFloat("_Distance", Vector3.Distance(origin.transform.position, transform.position));
            material.SetFloat("_Radius", radius);
            material.SetFloat("_Singularity", eventHorizon);
            material.SetFloat("_Sharpness", sharpness);
            material.SetVector("_Origin", pos);
            material.SetColor("_Colour", singularity);

            Graphics.Blit(source, destination, material);
        }
        else
            Graphics.Blit(source, destination);
    }
}
