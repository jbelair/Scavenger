using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Environment/Star")]
public class EnvironmentBasedStar : MonoBehaviour
{
    public Material[] starMaterials;

    public Statistics environment;
    public Material star;
    public Material corona;
    public Texture2D blackbodyRamp;
    public new Light light;
    public MeshRenderer coronaMesh;

    [System.Serializable]
    public class ParticleSystemContainer
    {
        public enum ColourFormat { Range, Linear, Exponent };
        public enum AlphaFormat { Pulse, LinearIn, LinearOut };

        public ParticleSystem system;
        public ColourFormat colour = ColourFormat.Linear;
        [Range(1, 10)]
        public float kelvinRange = 2;
        public bool invertGradient = false;
        public bool invertAlpha = false;
        public bool normaliseAlpha = false;
        public AlphaFormat alpha = AlphaFormat.LinearIn;
        //[Range(1, 50)]
        //public float sizeRange = 10;
    }

    public List<ParticleSystemContainer> particleSystems;

    public string statisticName;

    public float kelvin;
    public float kelvinRange;
    public float luminosity;

    // Use this for initialization
    void Start()
    {
        if (!star)
        {
            star = new Material(starMaterials[Random.Range(0, starMaterials.Length)]);
            MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();//.sharedMaterial 
            foreach (MeshRenderer mesh in meshes)
            {
                mesh.sharedMaterial = star;
            }
        }

        corona = new Material(corona);
        coronaMesh.sharedMaterial = corona;

        statisticName = name;

        if (environment == null)
            return;

        kelvin = environment[statisticName + " Kelvin"];
        kelvinRange = environment[statisticName + " Kelvin Range"];
        transform.localScale = Vector3.one * environment[statisticName + " Radius"];

        if (!blackbodyRamp)
            blackbodyRamp = (star.GetTexture("_Emissive") as Texture2D);

        luminosity = EnvironmentRules.StellarLuminosity(transform.localScale.x, kelvin + kelvinRange);

        Color colour = blackbodyRamp.GetPixelBilinear((kelvin + kelvinRange) / star.GetFloat("_KelvinMax"), 0);
        corona.SetColor("_TintColor", colour);

        if (light)
        {
            light.color = colour;
            light.intensity = Mathf.Log10(luminosity) / 4f;
        }

        Vector3 position = environment["System Coordinates"].Get<Vector3>();
        name = StringHelper.CoordinateName(position);

        int numberOfStars = environment["Stars"];
        int brightness = numberOfStars - 1;
        for (int i = 0; i < numberOfStars; i++)
        {
            if (luminosity > EnvironmentRules.StellarLuminosity(environment["Star " + StringHelper.IndexIntToChar(i) + " Radius"], (float)environment["Star " + StringHelper.IndexIntToChar(i) + " Kelvin"] + environment["Star " + StringHelper.IndexIntToChar(i) + " Kelvin Range"]))
                brightness--;
        }
        name += " " + StringHelper.IndexIntToChar(brightness);

        star.SetFloat("_Kelvin", kelvin);
        star.SetFloat("_KelvinRange", kelvinRange);
    }

    // Update is called once per frame
    public float lastKelvin = 0;
    public float lastKelvinRange = 0;
    public float lastKelvinMax = 0;
    public int lastParticleSystems = 0;
    void Update()
    {
        float kelvin = star.GetFloat("_Kelvin");
        float kelvinRange = star.GetFloat("_KelvinRange");
        float kelvinMax = star.GetFloat("_KelvinMax");

        if (lastKelvin != kelvin || lastKelvinRange != kelvinRange || lastKelvinMax != kelvinMax || lastParticleSystems != particleSystems.Count)
        {
            lastKelvin = kelvin;
            lastKelvinRange = kelvinRange;
            lastKelvinMax = kelvinMax;

            luminosity = EnvironmentRules.StellarLuminosity(transform.localScale.x, kelvin + kelvinRange);

            if (light)
                light.intensity = Mathf.Log10(luminosity) / 4f;

            lastParticleSystems = particleSystems.Count;
            foreach (ParticleSystemContainer container in particleSystems)
            {
                float percentage = (kelvin + kelvinRange * container.kelvinRange) / kelvinMax;

                Color colour100, colour75, colour50, colour25, colour0;
                colour100 = colour75 = colour50 = colour25 = colour0 = Color.white;

                switch(container.colour)
                {
                    case ParticleSystemContainer.ColourFormat.Range:
                        float percentageLow = (kelvin - kelvinRange * container.kelvinRange) / kelvinMax;
                        colour100 = blackbodyRamp.GetPixelBilinear(percentage, 0);
                        colour75 = blackbodyRamp.GetPixelBilinear(Mathf.Lerp(percentageLow, percentage, 0.75f), 0);
                        colour50 = blackbodyRamp.GetPixelBilinear(Mathf.Lerp(percentageLow, percentage, 0.5f), 0);
                        colour25 = blackbodyRamp.GetPixelBilinear(Mathf.Lerp(percentageLow, percentage, 0.25f), 0);
                        colour0 = blackbodyRamp.GetPixelBilinear(percentageLow, 0);
                        break;
                    case ParticleSystemContainer.ColourFormat.Linear:
                        colour100 = blackbodyRamp.GetPixelBilinear(percentage, 0);
                        colour75 = blackbodyRamp.GetPixelBilinear(percentage * 0.75f, 0);
                        colour50 = blackbodyRamp.GetPixelBilinear(percentage * 0.5f, 0);
                        colour25 = blackbodyRamp.GetPixelBilinear(percentage * 0.25f, 0);
                        colour0 = blackbodyRamp.GetPixelBilinear(0, 0);
                        break;
                    case ParticleSystemContainer.ColourFormat.Exponent:
                        colour100 = blackbodyRamp.GetPixelBilinear(percentage, 0);
                        colour75 = blackbodyRamp.GetPixelBilinear(percentage * 0.5f, 0);
                        colour50 = blackbodyRamp.GetPixelBilinear(percentage * 0.25f, 0);
                        colour25 = blackbodyRamp.GetPixelBilinear(percentage * 0.125f, 0);
                        colour0 = blackbodyRamp.GetPixelBilinear(0, 0);
                        break;
                }

                ParticleSystem.ColorOverLifetimeModule colour = container.system.colorOverLifetime;

                GradientColorKey[] colourKeys;
                GradientAlphaKey[] alphaKeys;

                float alphaMax = Mathf.Pow((kelvin + kelvinRange) / kelvinMax, 0.75f);
                if (container.normaliseAlpha)
                    alphaMax = 1;

                    if (container.invertGradient)
                {
                    colourKeys = new GradientColorKey[]
                    {
                        new GradientColorKey((colour0 * 8).Normalize(), 0),
                        new GradientColorKey((colour25 * 8).Normalize(), 0.25f),
                        new GradientColorKey((colour50 * 8).Normalize(), 0.5f),
                        new GradientColorKey((colour75 * 8).Normalize(), 0.75f),
                        new GradientColorKey((colour100 * 8).Normalize(), 1)
                    };
                }
                else
                {
                    colourKeys = new GradientColorKey[]
                    {
                        new GradientColorKey((colour100 * 8).Normalize(), 0),
                        new GradientColorKey((colour75 * 8).Normalize(), 0.25f),
                        new GradientColorKey((colour50 * 8).Normalize(), 0.5f),
                        new GradientColorKey((colour25 * 8).Normalize(), 0.75f),
                        new GradientColorKey((colour0 * 8).Normalize(), 1)
                    };
                }

                if (container.invertAlpha)
                {
                    switch (container.alpha)
                    {
                        case ParticleSystemContainer.AlphaFormat.LinearOut:
                            alphaKeys = new GradientAlphaKey[]
                            {
                                new GradientAlphaKey(alphaMax, 0),
                                new GradientAlphaKey(0, 1)
                            };
                            break;
                        case ParticleSystemContainer.AlphaFormat.Pulse:
                            alphaKeys = new GradientAlphaKey[]
                            {
                                new GradientAlphaKey(alphaMax, 0),
                                new GradientAlphaKey(0, 0.5f),
                                new GradientAlphaKey(alphaMax, 1)
                            };
                            break;
                        default:
                            alphaKeys = new GradientAlphaKey[]
                            {
                                new GradientAlphaKey(0, 0),
                                new GradientAlphaKey(alphaMax, 1)
                            };
                            break;
                    }
                }
                else
                {
                    switch (container.alpha)
                    {
                        case ParticleSystemContainer.AlphaFormat.LinearOut:
                            alphaKeys = new GradientAlphaKey[]
                            {
                                new GradientAlphaKey(0, 0),
                                new GradientAlphaKey(alphaMax, 1)
                            };
                            break;
                        case ParticleSystemContainer.AlphaFormat.Pulse:
                            alphaKeys = new GradientAlphaKey[]
                            {
                                new GradientAlphaKey(0, 0),
                                new GradientAlphaKey(alphaMax, 0.5f),
                                new GradientAlphaKey(0, 1)
                            };
                            break;
                        default:
                            alphaKeys = new GradientAlphaKey[]
                            {
                                new GradientAlphaKey(alphaMax, 0),
                                new GradientAlphaKey(0, 1)
                            };
                            break;
                    }
                }

                colour.color = new ParticleSystem.MinMaxGradient(new Gradient()
                {
                    colorKeys = colourKeys,
                    alphaKeys = alphaKeys
                });

                container.system.Clear();
                container.system.Simulate(container.system.main.duration);
                container.system.Play();
            }
        }
    }
}
