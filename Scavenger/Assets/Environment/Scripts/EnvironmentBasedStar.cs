using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Environment/Star")]
public class EnvironmentBasedStar : MonoBehaviour
{
    public Material[] starMaterials;

    public Statistics environment;
    public Material star;
    public Texture2D blackbodyRamp;
    public new Light light;

    [System.Serializable]
    public class ParticleSystemContainer
    {
        public ParticleSystem system;
        [Range(1, 10)]
        public float kelvinRange = 2;
        public bool invert = false;
        //[Range(1, 50)]
        //public float sizeRange = 10;
    }

    public ParticleSystemContainer[] particleSystems;

    public string statisticName;

    public float kelvin;
    public float kelvinRange;

    // Use this for initialization
    void Start()
    {
        if (!star)
        {
            star = new Material(starMaterials[Random.Range(0, starMaterials.Length)]);
            MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();//.sharedMaterial 
            foreach(MeshRenderer mesh in meshes)
            {
                mesh.sharedMaterial = star;
            }
        }

        statisticName = name;

        kelvin = environment[statisticName + " Kelvin"];
        kelvinRange = environment[statisticName + " Kelvin Range"];
        transform.localScale = Vector3.one * environment[statisticName + " Radius"];

        if (!blackbodyRamp)
            blackbodyRamp = (star.GetTexture("_Emissive") as Texture2D);

        light.color = blackbodyRamp.GetPixelBilinear(((kelvin + kelvinRange) / star.GetFloat("_KelvinMax")), 0);
        light.intensity = Mathf.Log10(EnvironmentRules.StellarLuminosity(transform.localScale.x, kelvin + kelvinRange)) / 4f;

        Vector3 position = environment["System Coordinates"].Get<Vector3>();
        if (position.y > 0)
            name = position.x + "+" + position.y;
        else
            name = position.x + "-" + position.y;

        int numberOfStars = environment["Stars"];
        int brightness = numberOfStars - 1;
        for (int i = 0; i < numberOfStars; i++)
        {
            if (light.intensity > Mathf.Log10(EnvironmentRules.StellarLuminosity(environment["Star " + StringHelper.IndexIntToChar(i) + " Radius"], (float)environment["Star " + StringHelper.IndexIntToChar(i) + " Kelvin"] + environment["Star " + StringHelper.IndexIntToChar(i) + " Kelvin Range"])) / 4f)
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
    void Update()
    {
        float kelvin = star.GetFloat("_Kelvin");
        float kelvinRange = star.GetFloat("_KelvinRange");
        float kelvinMax = star.GetFloat("_KelvinMax");

        if (lastKelvin != kelvin || lastKelvinRange != kelvinRange || lastKelvinMax != kelvinMax)
        {
            lastKelvin = kelvin;
            lastKelvinRange = kelvinRange;
            lastKelvinMax = kelvinMax;

            light.intensity = Mathf.Log10(EnvironmentRules.StellarLuminosity(transform.localScale.x, kelvin + kelvinRange)) / 4f;

            foreach (ParticleSystemContainer container in particleSystems)
            {
                Color colourHigh = blackbodyRamp.GetPixelBilinear(((kelvin + kelvinRange * container.kelvinRange) / kelvinMax), 0);
                Color colourMedium = light.color = blackbodyRamp.GetPixelBilinear(((kelvin + kelvinRange) / kelvinMax), 0);
                Color colourLow = blackbodyRamp.GetPixelBilinear(((kelvin / container.kelvinRange) / kelvinMax), 0);

                ParticleSystem.ColorOverLifetimeModule colour = container.system.colorOverLifetime;

                GradientColorKey[] colourKeys;
                GradientAlphaKey[] alphaKeys;

                if (container.invert)
                {
                    colourKeys = new GradientColorKey[]
                    {

                        new GradientColorKey((colourLow * 8).Normalize(), 1f),
                        new GradientColorKey((colourMedium * 8).Normalize(), 0.75f),
                        new GradientColorKey((colourHigh * 8).Normalize(), 0)
                    };

                    alphaKeys = new GradientAlphaKey[]
                    {
                        new GradientAlphaKey(0, 0),
                        new GradientAlphaKey(Mathf.Pow((kelvin + kelvinRange) / kelvinMax, 0.75f), 1)
                    };
                }
                else
                {
                    colourKeys = new GradientColorKey[]
                    {
                        new GradientColorKey((colourHigh * 8).Normalize(), 0),
                        new GradientColorKey((colourMedium * 8).Normalize(), 0.75f),
                        new GradientColorKey((colourLow * 8).Normalize(), 1f)
                    };

                    alphaKeys = new GradientAlphaKey[]
                    {
                        new GradientAlphaKey(Mathf.Pow((kelvin + kelvinRange) / kelvinMax, 0.75f), 0),
                        new GradientAlphaKey(0, 1)
                    };
                }

                colour.color = new ParticleSystem.MinMaxGradient(new Gradient()
                {
                    colorKeys = colourKeys,
                    alphaKeys = alphaKeys
                });
            }
        }
    }
}
