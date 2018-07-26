﻿using System.Collections.Generic;
using UnityEngine;

public class EnvironmentBasedPlanet : MonoBehaviour
{
    [System.Serializable]
    public class Sphere
    {
        public Material surface;
        public MeshRenderer sphere;
    }

    public enum Temperature { Cold, Warm, Hot };
    public enum Metallicity { Gasseous, Mixed, Metallic };

    public Statistics environment;
    public Temperature temperature;
    public Metallicity metallicity;
    public float kelvin = 0;
    public float kelvinLow = 0;
    public float kelvinHigh = 0;

    public Sphere surface;
    public Sphere atmosphere;

    public SpriteRenderer spriteCircle;
    public SpriteRenderer spriteIcon;

    public bool isInitialised = false;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInitialised)
        {//transform.localScale = Vector3.one * (Mathf.Log(environment[name + " Radius"] * 100f, 10) + 0.01f);
            isInitialised = true;
            Generate();
        }
    }
    
    public void Generate()
    {
        transform.localScale = Vector3.one * environment[name + " Radius"];

        environment.gameObject.GetComponent<PlanetGenerator>().GeneratePlanet(this);

        kelvin = environment[name + " Actual Kelvin"];
        kelvinLow = environment[name + " Actual Kelvin Low"];
        kelvinHigh = environment[name + " Actual Kelvin High"];
        float kelvinRange = kelvinHigh - kelvinLow;
        float intensity = environment[name + " Atmosphere Intensity"];
        float density = environment[name + " Atmosphere Density"];

        if (surface.surface)
        {
            surface.sphere.sharedMaterial = new Material(surface.surface);
            surface.sphere.sharedMaterial.SetFloat("_Kelvin", kelvin);
            surface.sphere.sharedMaterial.SetFloat("_KelvinRange", kelvinRange);
        }
        else
        {
            surface.sphere.gameObject.SetActive(false);
        }

        if (atmosphere.surface)
        {
            atmosphere.sphere.sharedMaterial = new Material(atmosphere.surface);
            atmosphere.sphere.sharedMaterial.SetFloat("_AtmosphereIntensity", intensity);
            atmosphere.sphere.sharedMaterial.SetFloat("_AtmosphereDensity", density);
            atmosphere.sphere.sharedMaterial.SetFloat("_Kelvin", kelvin);
            atmosphere.sphere.sharedMaterial.SetFloat("_KelvinRange", kelvinRange);
        }
        else
        {
            atmosphere.sphere.gameObject.SetActive(false);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        List<ParticleCollisionEvent> particleCollisions = new List<ParticleCollisionEvent>();
        ParticlePhysicsExtensions.GetCollisionEvents(other.GetComponent<ParticleSystem>(), gameObject, particleCollisions);
    }
}
